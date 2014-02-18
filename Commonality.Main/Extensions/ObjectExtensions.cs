using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Commonality.Main.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Turns the object into an ExpandoObject
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static dynamic ToExpando(this object o)
        {
            var result = new ExpandoObject();
            var d = result as IDictionary<string, object>;
            if (o is ExpandoObject) return o;
            if (o.GetType() == typeof(NameValueCollection)
                || o.GetType().IsSubclassOf(typeof(NameValueCollection)))
            {
                var nv = (NameValueCollection)o;
                nv.Cast<string>().Select(key => new KeyValuePair<string, object>(key, nv[key]))
                    .ToList().ForEach(d.Add);
            }
            else
            {
                var props = o.GetType().GetProperties();
                foreach (var item in props.Where(p => p.GetIndexParameters().Count() == 0))
                {
                    d.Add(item.Name, item.GetValue(o, null));
                }
            }
            return result;
        }

        /// <summary>
        /// Maps one object into a new instance of another. Copies values from the identically named
        /// properties of one item into the other.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="sourceItem">The source item.</param>
        /// <returns></returns>
        public static D MapTo<D, S>(this S sourceItem)
        {
            return sourceItem.MakeMapperFunc<S, D>()(sourceItem);
        }

        /// <summary>
        /// Makes a mapper function that maps all values of the properties of one object into another.
        /// </summary>
        /// <typeparam name="S">Source item</typeparam>
        /// <typeparam name="D">Destination item</typeparam>
        /// <param name="sourceItem">The source item.</param>
        /// <returns></returns>
        public static Func<S, D> MakeMapperFunc<S, D>(this S sourceItem)
        {
            return MakeMapperFunc<S, D>();
        }

        /// <summary>
        /// Makes a mapper function that maps all values of the properties of one object into another.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="D"></typeparam>
        /// <returns></returns>
        public static Func<S, D> MakeMapperFunc<S, D>()
        {
            var source = Expression.Parameter(typeof(S), "source");
            var dest = Expression.Variable(typeof(D), "dest");
            var assignments = from srcProp in
                                  typeof(S).GetProperties(
                                      BindingFlags.Public |
                                      BindingFlags.Instance)
                              where srcProp.CanRead
                              let destProp = typeof(D).
                                  GetProperty(srcProp.Name,
                                              BindingFlags.Public |
                                              BindingFlags.Instance)
                              where (destProp.CanWrite)
                              select Expression.Assign(Expression.Property(dest, destProp),
                                                       Expression.Property(source, srcProp));
            var body = new List<Expression>
                           {
                               Expression.Assign(dest, Expression.New(typeof (D)))
                           };
            body.AddRange(assignments);
            body.Add(dest);
            var expr = Expression.Lambda<Func<S, D>>(Expression.Block(new[] { dest },
                                                                      body.ToArray()), source);
            return expr.Compile();
        }

        /// <summary>
        /// Maps the values of the properties of one object into the properties of another.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="D"></typeparam>
        /// <param name="sourceItem">The source item.</param>
        /// <param name="destination">The destination.</param>
        public static void MapInto<S, D>(this S sourceItem, D destination)
            where S : class
            where D : class
        {
            if (sourceItem == null || destination == null)
                return;

            var destType = typeof(D);
            typeof(S).GetProperties().Where(p => !p.GetIndexParameters().Any())
                .Select(p => new
                {
                    Prop = p,
                    SourceValue = p.GetValue(sourceItem, null),
                    DestType = destType.GetProperty(p.Name)
                })
                .Where(p => p.SourceValue != null && p.DestType != null)
                .ForEach(p => destination.BindValueToProperty(p.SourceValue, p.DestType));
        }


        /// <summary>
        /// Returns an expando as a dictionary.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IDictionary<string, object> AsDictionary(this ExpandoObject input)
        {
            return input;
        }

        /// <summary>
        /// Turns an expando into a string dictionary.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IDictionary<string, string> AsStringDictionary(this ExpandoObject input)
        {
            return input.ToDictionary(k => k.Key, v => (v.Value ?? "").ToString());
        }

        /// <summary>
        /// Returns a property value from an expando.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static object ExpandoObjectValue(this ExpandoObject o, string field)
        {
            return o.AsDictionary()[field];
        }

        /// <summary>
        /// Populates an object from an expando
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="mapFrom">The map from.</param>
        /// <returns></returns>
        public static object PopulateFromExpando(this object o, ExpandoObject mapFrom)
        {
            var availableValues = mapFrom as IDictionary<string, object>;

            foreach (var prop in o.GetType().GetProperties()
                .Where(prop => availableValues.ContainsKey(prop.Name) && prop.CanWrite))
            {
                prop.SetValue(o, availableValues[prop.Name], null);
            }

            return o;
        }

        /// <summary>
        /// Populates an object from a collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static object PopulateFrom<T>(this T o, NameValueCollection collection)
        {
            var type = typeof(T);

            foreach (var item in collection.AllKeys)
            {
                var property = type.GetProperty(item,
                    BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic);
                if (property == null)
                    continue;
                BindValueToProperty(o, collection[item], property);
            }
            return o;
        }

        /// <summary>
        /// Binds a value to a property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="bindValue">The bind value.</param>
        /// <param name="expression">The expression.</param>
        public static void BindValueToProperty<T, TResult>(this T target, object bindValue,
                                                           Expression<Func<T, TResult>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                return;

            var prop = memberExpression.Member as PropertyInfo;
            if (prop == null)
                return;

            BindValueToProperty(target, bindValue, prop);
        }


        /// <summary>
        /// Binds a value to a property.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="bindValue">The bind value.</param>
        /// <param name="propertyName">Name of the property.</param>
        public static void BindValueToProperty(this object target, object bindValue, string propertyName)
        {
            PropertyInfo prop = target.GetType().GetProperty(propertyName);
            if (prop == null)
                return;

            BindValueToProperty(target, bindValue, prop);
        }


        /// <summary>
        /// Gets the value from a property.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static object GetValueFromProperty(this object target, string propertyName,
            object[] index = null)
        {
            var prop = target.GetType().GetProperty(propertyName);
            return prop == null ? null : prop.GetValue(target, index);
        }

        /// <summary>
        /// Binds a value to a property.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="bindValue">The bind value.</param>
        /// <param name="prop">The prop.</param>
        public static void BindValueToProperty(this object target, object bindValue, PropertyInfo prop)
        {
            if (bindValue == null)
            {
                // Possible exception if not nullable.
                prop.SetValue(target, null, null);
                return;
            }

            if (prop.PropertyType == bindValue.GetType())
            {
                // types are same. no probs
                prop.SetValue(target, bindValue, null);
                return;
            }

            var valueType = bindValue.GetType();

            // check for a string parse method
            var parseMethod = prop.PropertyType.GetMethod("Parse", new[] { valueType });
            if (parseMethod != null)
            {
                parseMethod.Invoke(null, new[] { bindValue });
                return;
            }

            // last effort
            var converter = TypeDescriptor.GetConverter(prop.PropertyType);
            var conversionAttempt = converter == null ? null : bindValue is string
                                           ? converter.ConvertFromString(bindValue.ToString())
                                           : converter.ConvertFrom(bindValue);

            if (conversionAttempt != null)
                prop.SetValue(target, conversionAttempt, null);
            else
            {
                // Is a nullable item?
                if (Nullable.GetUnderlyingType(prop.PropertyType) != null)
                    prop.SetValue(target, null, null);
                else
                    throw new InvalidOperationException("Unable to bind value " + prop.Name);
            }
        }
    }
}
