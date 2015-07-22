/**
 * Copyright 2015 Marc Rufer, d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachine
{
    public class StateMachine
    {

        protected HashSet<String> ConditionSet = new HashSet<String>(StringComparer.OrdinalIgnoreCase);

        protected StateMachine AddCondition(String name)
        {
            lock (_transitions)
            {
                var fReturn = ConditionSet.Contains(name, StringComparer.OrdinalIgnoreCase);
                if (fReturn)
                {
                    throw new ArgumentException(String.Format("Machine condition already exists: '{0}'", name), "name");
                }
                ConditionSet.Add(name);
            }
            return this;
        }

        protected StateMachine AddConditions(IEnumerable<String> names, bool ignoreExisting = false)
        {
            lock (_transitions)
            {
                var fReturn = ConditionSet.Overlaps(names);
                if (fReturn && !ignoreExisting)
                {
                    throw new ArgumentException(String.Format("Machine states already exist: '{0}'", String.Join(", ", StateSet.Intersect(names))), "names");
                }
                foreach (var name in names)
                {
                    ConditionSet.Add(name);
                }
            }
            return this;
        }

        protected HashSet<String> StateSet = new HashSet<String>(StringComparer.OrdinalIgnoreCase);

        protected StateMachine AddState(String name)
        {
            lock (_transitions)
            {
                var fReturn = StateSet.Contains(name);
                if (fReturn)
                {
                    throw new ArgumentException(String.Format("Machine state already exists: '{0}'", name), "name");
                }
                StateSet.Add(name);
            }
            return this;
        }

        protected StateMachine AddStates(IEnumerable<String> names, bool ignoreExisting = false)
        {
            lock (_transitions)
            {
                var fReturn = StateSet.Overlaps(names);
                if (fReturn && !ignoreExisting)
                {
                    throw new ArgumentException(String.Format("Machine states already exist: '{0}'", String.Join(", ", StateSet.Intersect(names))), "names");
                }
                foreach (var name in names)
                {
                    StateSet.Add(name);
                }
            }
            return this;
        }

        protected class StateTransition
        {
            readonly String CurrentState;
            readonly String Condition;

            public StateTransition(String sourceState, String condition)
            {
                CurrentState = sourceState;
                Condition = condition;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Condition.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState.Equals(other.CurrentState, StringComparison.OrdinalIgnoreCase) && this.Condition.Equals(other.Condition, StringComparison.OrdinalIgnoreCase);
            }
            public override String ToString()
            {
                return String.Format("{0}-{1}", CurrentState, Condition);
            }
        }

        Dictionary<StateTransition, String> _transitions = new Dictionary<StateTransition, String>();
        public String CurrentState { get; protected set; }
        public String PreviousState { get; protected set; }

        public String InitialState
        {
            get
            {
                return "Created";
            }
            protected set { }
        }
        public virtual bool IsInitialState
        {
            get
            {
                return CurrentState.Equals(InitialState, StringComparison.OrdinalIgnoreCase);
            }
        }
        public String RunningState
        {
            get
            {
                return "Running";
            }
            protected set { }
        }
        public String ErrorState
        {
            get
            {
                return "InternalErrorState";
            }
            protected set { }
        }
        public String CompletedState
        {
            get
            {
                return "Completed";
            }
            protected set { }
        }
        public String CancelledState
        {
            get
            {
                return "Cancelled";
            }
            protected set { }
        }
        public String FinalState
        {
            get
            {
                return "Disposed";
            }
            protected set { }
        }
        public virtual bool IsFinalState
        {
            get
            {
                return CurrentState.Equals(FinalState, StringComparison.OrdinalIgnoreCase);
            }
        }
        public String ContinueCondition
        {
            get
            {
                return "Continue";
            }
            protected set { }
        }
        public String CancelCondition
        {
            get
            {
                return "Cancel";
            }
            protected set { }
        }

        public StateMachine()
        {
            InitStateMachine();
        }

        private void InitStateMachine()
        {
            CurrentState = InitialState;
            PreviousState = InitialState;

            AddStates(new List<String> { InitialState, RunningState, CompletedState, CancelledState, ErrorState, FinalState });
            AddConditions(new List<String> { ContinueCondition, CancelCondition });

            SetStateTransition(InitialState, ContinueCondition, RunningState);
            SetStateTransition(InitialState, CancelCondition, ErrorState);
            SetStateTransition(RunningState, ContinueCondition, CompletedState);
            SetStateTransition(RunningState, CancelCondition, CancelledState);
            SetStateTransition(CompletedState, ContinueCondition, FinalState);
            SetStateTransition(CompletedState, CancelCondition, ErrorState);
            SetStateTransition(CancelledState, ContinueCondition, FinalState);
            SetStateTransition(CancelledState, CancelCondition, ErrorState);
            SetStateTransition(ErrorState, ContinueCondition, FinalState);
        }

        protected StateMachine SetStateTransition(String sourceState, String condition, String targetState, bool fReplace = false)
        {
            lock (_transitions)
            {
                if (!StateSet.Contains(sourceState))
                {
                    throw new KeyNotFoundException(String.Format("sourceState not found: '{0}'", sourceState));
                }
                if (!StateSet.Contains(targetState))
                {
                    throw new KeyNotFoundException(String.Format("targetState not found: '{0}'", targetState));
                }
                if (!ConditionSet.Contains(condition))
                {
                    throw new KeyNotFoundException(String.Format("condition not found: '{0}'", condition));
                }
                String _processState;
                var stateTransition = new StateTransition(sourceState, condition);
                var fReturn = _transitions.TryGetValue(stateTransition, out _processState);
                if (fReturn)
                {
                    if (fReplace)
                    {
                        _transitions.Remove(stateTransition);
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("stateTransition already exists: '{0}' -- > '{1}'", sourceState, condition));
                    }
                }
                _transitions.Add(stateTransition, targetState);
            }
            return this;
        }

        protected StateMachine InsertStateTransition(String sourceState, String condition, String targetStateNew, bool fCreateTargetState = false)
        {
            lock (_transitions)
            {
                if (!StateSet.Contains(sourceState))
                {
                    throw new KeyNotFoundException(String.Format("sourceState not found: '{0}'", sourceState));
                }
                if (!StateSet.Contains(targetStateNew))
                {
                    if (!fCreateTargetState)
                    {
                        throw new KeyNotFoundException(String.Format("targetStateNew not found: '{0}'", targetStateNew));
                    }
                    AddState(targetStateNew);
                }
                if (!ConditionSet.Contains(condition))
                {
                    throw new KeyNotFoundException(String.Format("condition not found: '{0}'", condition));
                }
                String _processState;
                var stateTransition = new StateTransition(sourceState, condition);
                var fReturn = _transitions.TryGetValue(stateTransition, out _processState);
                if (!fReturn)
                {
                    throw new ArgumentException(String.Format("stateTransition not found: '{0}' -- > '{1}'", sourceState, condition));
                }
                _transitions.Remove(stateTransition);
                _transitions.Add(stateTransition, targetStateNew);
                var stateTransitionNew = new StateTransition(targetStateNew, condition);
                _transitions.Add(stateTransitionNew, _processState);
            }
            return this;
        }

        // DFTODO check, if replacing JavaScriptSearializer with Netwonsoft Json lib makes sense
        public virtual String GetStateMachine()
        {
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            lock (_transitions)
            {
                var dic = _transitions.ToDictionary(k => k.Key.ToString(), v => v.Value);
                var stateMachineSerialised = jss.Serialize(dic);
                return stateMachineSerialised;
            }
        }

        // DFTODO create constructor (public, which calls the private no arg constructor and takes the arguments provided here)
        public virtual bool SetStateMachine(String configuration, String currentState = null, String previousState = null)
        {
            var fReturn = false;
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            lock (_transitions)
            {
                Dictionary<String, String> dic = jss.Deserialize<Dictionary<String, String>>(configuration);
                _transitions.Clear();
                StateSet.Clear();
                ConditionSet.Clear();
                foreach (KeyValuePair<String, String> item in dic)
                {
                    var sourceStateCondition = item.Key.Split('-');
                    var sourceState = sourceStateCondition.First();
                    var condition = sourceStateCondition.Last();
                    var targetState = item.Value.ToString();
                    ConditionSet.Add(condition);
                    StateSet.Add(sourceState);
                    StateSet.Add(targetState);
                    SetStateTransition(sourceState, condition, targetState, true);
                }
                if (null != currentState)
                {
                    if (!StateSet.Contains(currentState))
                    {
                        throw new ArgumentOutOfRangeException("currentState", String.Format("currentState: Parameter validation failed. '{0}' is not a valid state.", currentState));
                    }
                    CurrentState = currentState;
                }
                if (null != previousState)
                {
                    if (!StateSet.Contains(previousState))
                    {
                        throw new ArgumentOutOfRangeException("previousState", String.Format("previousState: Parameter validation failed. '{0}' is not a valid state.", previousState));
                    }
                    PreviousState = previousState;
                }
                fReturn = true;
            }
            return fReturn;
        }

        protected virtual void Clear()
        {
            lock (_transitions)
            {
                _transitions.Clear();
                StateSet.Clear();
                ConditionSet.Clear();
            }
        }

        public String GetNext(String condition)
        {
            StateTransition transition = new StateTransition(CurrentState, condition);
            String _nextState;
            lock (_transitions)
            {
                if (!_transitions.TryGetValue(transition, out _nextState))
                {
                    throw new ArgumentOutOfRangeException(String.Format("stateTransition is invalid: '{0}' @ '{1}'", CurrentState, condition));
                }
            }
            return _nextState;
        }

        public String MoveNext() { return Continue(); }
        public String Continue() { return MoveNext(ContinueCondition); }
        public String Cancel() { return MoveNext(CancelCondition); }

        public String MoveNext(String condition)
        {
            lock (_transitions)
            {
                // DFTCHECK Rename GetNext method to a more meaningful name
                String _nextState = GetNext(condition);
                PreviousState = CurrentState;
                CurrentState = _nextState;
            }
            return CurrentState;
        }
    }
}
