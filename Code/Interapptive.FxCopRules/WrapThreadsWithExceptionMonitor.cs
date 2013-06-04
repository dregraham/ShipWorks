using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.FxCop.Sdk;
using System.Reflection;
using System.Diagnostics;

namespace Interapptive.FxCopRules
{
    public class WrapThreadsWithExceptionMonitor : BaseIntrospectionRule
    {
        public WrapThreadsWithExceptionMonitor() :
            base("WrapThreadsWithExceptionMonitor", "Interapptive.FxCopRules.Rules.xml", Assembly.GetExecutingAssembly())
        {

        }

        /// <summary>
        /// Check all member methods
        /// </summary>
        public override ProblemCollection Check(Member member)
        {
            if (member is Method)
            {
                VisitStatements(((Method) member).Body.Statements);
            }

            return this.Problems;
        }

        /// <summary>
        /// Calls to ThreadPool.QueueUserWorkItem will show up as MemberBinding instances of MethodCalls
        /// </summary>
        public override void VisitMethodCall(MethodCall call)
        {
            MemberBinding memberBinding = call.Callee as MemberBinding;

            if (memberBinding.BoundMember.DeclaringType.FullName == "System.Threading.ThreadPool" && memberBinding.BoundMember.Name.Name == "QueueUserWorkItem")
            {
                CheckOperandIsExceptionMonitor(call.Operands[0], call);
            }

            base.VisitMethodCall(call);
        }

        /// <summary>
        /// Calls to "new Thread" will show up as Construct instances
        /// </summary>
        public override void VisitConstruct(Construct construct)
        {
            if (construct.Type.FullName == "System.Threading.Thread")
            {
                CheckOperandIsExceptionMonitor(construct.Operands[0], construct);
            }

            base.VisitConstruct(construct);
        }

        /// <summary>
        /// Check that the given operand comes from making a call to ExceptionMonitor
        /// </summary>
        private void CheckOperandIsExceptionMonitor(Expression operand, Node target)
        {
            bool pass = false;

            MethodCall methodCall = operand as MethodCall;
            if (methodCall != null)
            {
                MemberBinding callee = methodCall.Callee as MemberBinding;
                if (callee != null)
                {
                    if (callee.BoundMember.DeclaringType.Name.Name == "ExceptionMonitor")
                    {
                        pass = true;
                    }
                }
            }

            if (!pass)
            {
                this.Problems.Add(new Problem(GetResolution(), target));
            }
        }

    }
}
