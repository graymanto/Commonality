using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using Commonality.Main.Extensions;

namespace Commonality.Main.Contracts
{
    public class Require
    {
        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(
            Expression<Func<T>> expression, 
            string message = "Argument must not be null") where T : class
        {
            Debug.Assert(expression != null, "Expression must be provided to ArgumentNotNull function");
            Debug.Assert(
                expression.Body is MemberExpression, 
                "Please pass a member expression to  ArgumentNotNull function");

            var expressionValue = expression.Compile().Invoke();
            if (expressionValue != null)
            {
                return;
            }

            var memberExpression = (MemberExpression)expression.Body;
            throw new ArgumentNullException(memberExpression.Member.Name, message);
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(T value, string paramName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNullOrEmpty<T>(IEnumerable<T> value, string paramName) where T : class
        {
            if (value == null || !value.Any())
            {
                throw new ArgumentOutOfRangeException(paramName, @"Argument must not be null or empty.");
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentIsOfType<T>(T input, Type typeToMatch, string paramName)
        {
            var inputType = input.GetType();
            if (inputType.IsGenericType)
            {
                inputType = inputType.GetGenericTypeDefinition();
            }

            if (inputType != typeToMatch)
            {
                throw new ArgumentException(
                    "Parameter is of type {0} and not the required {1}".Formatted(inputType, typeToMatch), 
                    paramName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentIsInRange(int input, int min, int max, string paramName)
        {
            if (input < min || input > max)
            {
                throw new ArgumentOutOfRangeException(
                    paramName, 
                    "Argument value {0} is not in the range {1} to {2}".Formatted(input, min, max));
            }
        }
    }
}