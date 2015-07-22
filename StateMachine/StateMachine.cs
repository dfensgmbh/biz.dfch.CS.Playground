﻿/**
 * Copyright 2014-2015 Ronald Rink, d-fens GmbH
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

namespace LightSwitchApplication
{
    public class StateMachine
    {

        protected HashSet<string> ConditionSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        protected StateMachine AddCondition(string name)
        {
            lock (_transitions)
            {
                var fReturn = ConditionSet.Contains(name, StringComparer.OrdinalIgnoreCase);
                if (fReturn)
                {
                    throw new ArgumentException(string.Format("Machine condition already exists: '{0}'", name), "name");
                }
                ConditionSet.Add(name);
            }
            return this;
        }
        protected StateMachine AddConditions(IEnumerable<string> names, bool ignoreExisting = false)
        {
            lock (_transitions)
            {
                var fReturn = ConditionSet.Overlaps(names);
                if (fReturn && !ignoreExisting)
                {
                    throw new ArgumentException(string.Format("Machine states already exist: '{0}'", string.Join(", ", StateSet.Intersect(names))), "names");
                }
                foreach (var name in names)
                {
                    ConditionSet.Add(name);
                }
            }
            return this;
        }

        protected HashSet<string> StateSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        protected StateMachine AddState(string name)
        {
            lock (_transitions)
            {
                var fReturn = StateSet.Contains(name);
                if (fReturn)
                {
                    throw new ArgumentException(string.Format("Machine state already exists: '{0}'", name), "name");
                }
                StateSet.Add(name);
            }
            return this;
        }
        protected StateMachine AddStates(IEnumerable<string> names, bool ignoreExisting = false)
        {
            lock (_transitions)
            {
                var fReturn = StateSet.Overlaps(names);
                if (fReturn && !ignoreExisting)
                {
                    throw new ArgumentException(string.Format("Machine states already exist: '{0}'", string.Join(", ", StateSet.Intersect(names))), "names");
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
            readonly string CurrentState;
            readonly string Condition;

            public StateTransition(string sourceState, string condition)
            {
                //if()
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
            public override string ToString()
            {
                return string.Format("{0}-{1}", CurrentState, Condition);
            }
        }

        Dictionary<StateTransition, string> _transitions = new Dictionary<StateTransition, string>();
        public string CurrentState { get; protected set; }
        public string PreviousState { get; protected set; }

        public string InitialState
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
        public string RunningState
        {
            get
            {
                return "Running";
            }
            protected set { }
        }
        public string ErrorState
        {
            get
            {
                return "InternalErrorState";
            }
            protected set { }
        }
        public string CompletedState
        {
            get
            {
                return "Completed";
            }
            protected set { }
        }
        public string CancelledState
        {
            get
            {
                return "Cancelled";
            }
            protected set { }
        }
        public string FinalState
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
        public string ContinueCondition
        {
            get
            {
                return "Continue";
            }
            protected set { }
        }
        public string CancelCondition
        {
            get
            {
                return "Cancel";
            }
            protected set { }
        }

        public StateMachine()
        {
            CurrentState = InitialState;
            PreviousState = InitialState;

            AddStates(new List<string> { InitialState, RunningState, CompletedState, CancelledState, ErrorState, FinalState });
            AddConditions(new List<string> { ContinueCondition, CancelCondition });

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

        protected StateMachine SetStateTransition(string sourceState, string condition, string targetState, bool fReplace = false)
        {
            lock (_transitions)
            {
                if (!StateSet.Contains(sourceState))
                {
                    throw new KeyNotFoundException(string.Format("sourceState not found: '{0}'", sourceState));
                }
                if (!StateSet.Contains(targetState))
                {
                    throw new KeyNotFoundException(string.Format("targetState not found: '{0}'", targetState));
                }
                if (!ConditionSet.Contains(condition))
                {
                    throw new KeyNotFoundException(string.Format("condition not found: '{0}'", condition));
                }
                string _processState;
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
                        throw new ArgumentException(string.Format("stateTransission already exists: '{0}' -- > '{1}'", sourceState, condition), "stateTransission");
                    }
                }
                _transitions.Add(stateTransition, targetState);
            }
            return this;
        }

        protected StateMachine InsertStateTransition(string sourceState, string condition, string targetStateNew, bool fCreateTargetState = false)
        {
            lock (_transitions)
            {
                if (!StateSet.Contains(sourceState))
                {
                    throw new KeyNotFoundException(string.Format("sourceState not found: '{0}'", sourceState));
                }
                if (!StateSet.Contains(targetStateNew))
                {
                    if (!fCreateTargetState)
                    {
                        throw new KeyNotFoundException(string.Format("targetStateNew not found: '{0}'", targetStateNew));
                    }
                    AddState(targetStateNew);
                }
                if (!ConditionSet.Contains(condition))
                {
                    throw new KeyNotFoundException(string.Format("condition not found: '{0}'", condition));
                }
                string _processState;
                var stateTransition = new StateTransition(sourceState, condition);
                var fReturn = _transitions.TryGetValue(stateTransition, out _processState);
                if (!fReturn)
                {
                    throw new ArgumentException(string.Format("stateTransission not found: '{0}' -- > '{1}'", sourceState, condition), "stateTransission");
                }
                _transitions.Remove(stateTransition);
                _transitions.Add(stateTransition, targetStateNew);
                var stateTransitionNew = new StateTransition(targetStateNew, condition);
                _transitions.Add(stateTransitionNew, _processState);
            }
            return this;
        }

        public virtual string GetStateMachine()
        {
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            lock (_transitions)
            {
                var dic = _transitions.ToDictionary(k => k.Key.ToString(), v => v.Value);
                var stateMachineSerialised = jss.Serialize(dic);
                return stateMachineSerialised;
            }
        }
        public virtual bool SetStateMachine(string configuration, string currentState = null, string previousState = null)
        {
            var fReturn = false;
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            lock (_transitions)
            {
                Dictionary<string, string> dic = jss.Deserialize<Dictionary<string, string>>(configuration);
                _transitions.Clear();
                StateSet.Clear();
                ConditionSet.Clear();
                foreach (KeyValuePair<string, string> item in dic)
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
                        throw new ArgumentOutOfRangeException("currentState", string.Format("currentState: Parameter validation failed. '{0}' is not a valid state.", currentState));
                    }
                    CurrentState = currentState;
                }
                if (null != previousState)
                {
                    if (!StateSet.Contains(previousState))
                    {
                        throw new ArgumentOutOfRangeException("previousState", string.Format("previousState: Parameter validation failed. '{0}' is not a valid state.", previousState));
                    }
                    PreviousState = previousState;
                }
                fReturn = true;
            }
            return fReturn;
        }
        protected virtual void ClearStateMachine()
        {
            lock (_transitions)
            {
                _transitions.Clear();
                StateSet.Clear();
                ConditionSet.Clear();
            }
        }

        public string GetNext(string condition)
        {
            StateTransition transition = new StateTransition(CurrentState, condition);
            string _nextState;
            lock (_transitions)
            {
                if (!_transitions.TryGetValue(transition, out _nextState))
                {
                    throw new ArgumentOutOfRangeException(string.Format("stateTransission is invalid: '{0}' @ '{1}'", CurrentState, condition));
                }
            }
            return _nextState;
        }

        public string MoveNext()
        {
            return Continue();
        }
        public string Continue()
        {
            return MoveNext(ContinueCondition);
        }
        public string Cancel()
        {
            return MoveNext(CancelCondition);
        }
        public string MoveNext(string condition)
        {
            lock (_transitions)
            {
                string _nextState = GetNext(condition);
                PreviousState = CurrentState;
                CurrentState = _nextState;
            }
            return CurrentState;
        }

    }
}
