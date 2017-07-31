using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace WindowsFormsApp1.StackTraceHelper
{
    /// <summary>
	/// More lightweight analog to StackTrace
	/// </summary>
	public class StackTraceSlim
    {
        private static readonly Action<object> getStackFramesInternal;
        private static readonly ConcurrentDictionary<IntPtr, MethodBaseSlim> methods = new ConcurrentDictionary<IntPtr, MethodBaseSlim>();

        /// <summary>
        /// Creates necessary delegate
        /// </summary>
        static StackTraceSlim()
        {
            var getStackFramesInternalMethod = typeof(StackTrace).GetMethod("GetStackFramesInternal", BindingFlags.NonPublic | BindingFlags.Static);

            var stackFrameHelperParam = Expression.Parameter(typeof(object));

            getStackFramesInternal = Expression.Lambda<Action<Object>>(Expression.Call(getStackFramesInternalMethod,
                Expression.Convert(stackFrameHelperParam, StackFrameHelperProxy.UnderlyingType),
                Expression.Constant(0),
                Expression.Constant(null, typeof(Exception))),
                MethodNameHelper.GetLambdaMethodName(),
                new[] {
                stackFrameHelperParam
                }).Compile();
        }

        public StackTraceSlim(StackFrameSlim[] frames)
        {
            Frames = frames;
        }

        /// <summary>
        /// Creates StackTraceSlim and fills its frames
        /// </summary>
        /// <param name="needFileLineColInfo">Whether source code information should be extracted</param>
        public StackTraceSlim(bool needFileLineColInfo)
        {
            var sfh = new StackFrameHelperProxy(needFileLineColInfo);

            getStackFramesInternal(sfh.UnderlyingInstance);

            Frames = new StackFrameSlim[sfh.GetNumberOfFrames() - 1];
            for (var i = 0; i < Frames.Length; i++)
            {
                Frames[i] = sfh.CreateFrame(i);
                Frames[i].Method = GetMethodForHandle(Frames[i].MethodHandle);
            }
        }

        public StackFrameSlim[] Frames;

        /// <summary>
        /// Finds method by handle, first looking at cached methods table
        /// </summary>
        /// <param name="methodHandle">Handle</param>
        /// <returns>MethodBaseSlim</returns>
        private MethodBaseSlim GetMethodForHandle(IntPtr methodHandle)
        {
            MethodBaseSlim method;

            if (!methods.TryGetValue(methodHandle, out method))
            {
                method = new MethodBaseSlim(StackFrameHelperProxy.GetMethod(methodHandle));

                methods.AddOrUpdate(methodHandle, method, (k, v) => v);
            }

            return method;
        }
    }
}