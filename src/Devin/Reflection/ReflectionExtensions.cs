﻿using System.Reflection;
using System.Runtime.CompilerServices;

namespace Devin.Reflection
{
    public static class ReflectionExtensions
    {
        public static bool IsNonAbstractClass(this Type type, bool publicOnly)
        {
            if (type.IsSpecialName)
            {
                return false;
            }

            if (type.IsClass && !type.IsAbstract)
            {
                if (type.HasAttribute<CompilerGeneratedAttribute>())
                {
                    return false;
                }

                if (publicOnly)
                {
                    return type.IsPublic || type.IsNestedPublic;
                }

                return true;
            }

            return false;
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            foreach (var implementedInterface in type.GetInterfaces())
            {
                yield return implementedInterface;
            }

            var baseType = type.BaseType;
            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.BaseType;
            }
        }

        public static bool IsInNamespace(this Type type, string @namespace)
        {
            var typeNamespace = type.Namespace ?? string.Empty;

            if (@namespace.Length > typeNamespace.Length)
            {
                return false;
            }

            var typeSubNamespace = typeNamespace.Substring(0, @namespace.Length);

            if (typeSubNamespace.Equals(@namespace, StringComparison.Ordinal))
            {
                if (typeNamespace.Length == @namespace.Length)
                {
                    //exactly the same
                    return true;
                }

                //is a subnamespace?
                return typeNamespace[@namespace.Length] == '.';
            }

            return false;
        }

        public static bool IsInExactNamespace(this Type type, string @namespace)
        {
            return string.Equals(type.Namespace, @namespace, StringComparison.Ordinal);
        }

        public static bool HasAttribute(this Type type, Type attributeType)
        {
            return type.IsDefined(attributeType, inherit: true);
        }

        public static bool HasAttribute<T>(this Type type) where T : Attribute
        {
            return type.HasAttribute(typeof(T));
        }

        public static bool HasAttribute<T>(this Type type, Func<T, bool> predicate) where T : Attribute
        {
            return type.GetCustomAttributes<T>(inherit: true).Any(predicate);
        }

        public static bool IsBasedOn(this Type type, Type otherType)
        {
            if (otherType.IsGenericTypeDefinition)
            {
                return type.IsAssignableToGenericTypeDefinition(otherType);
            }

            return otherType.IsAssignableFrom(type);
        }

        public static bool IsBasedOn<T>(this Type type)
        {
            var otherType = typeof(T);

            return type.IsBasedOn(otherType);
        }

        private static bool IsAssignableToGenericTypeDefinition(this Type type, Type genericType)
        {
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType)
                {
                    var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == genericType)
                    {
                        return true;
                    }
                }
            }

            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == genericType)
                {
                    return true;
                }
            }

            var baseType = type.BaseType;
            if (baseType is null)
            {
                return false;
            }

            return baseType.IsAssignableToGenericTypeDefinition(genericType);
        }

        private static IEnumerable<Type> GetImplementedInterfacesToMap(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.GetInterfaces();
            }

            if (!type.IsGenericTypeDefinition)
            {
                return type.GetInterfaces();
            }

            return FilterMatchingGenericInterfaces(type);
        }

        private static IEnumerable<Type> FilterMatchingGenericInterfaces(Type type)
        {
            var genericArguments = type.GetGenericArguments();

            foreach (var current in type.GetInterfaces())
            {
                if (current.IsGenericType && current.ContainsGenericParameters && GenericParametersMatch(genericArguments, current.GetGenericArguments()))
                {
                    yield return current.GetGenericTypeDefinition();
                }
            }
        }

        private static bool GenericParametersMatch(IReadOnlyList<Type> parameters, IReadOnlyList<Type> interfaceArguments)
        {
            if (parameters.Count != interfaceArguments.Count)
            {
                return false;
            }

            for (var i = 0; i < parameters.Count; i++)
            {
                if (parameters[i] != interfaceArguments[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsOpenGeneric(this Type type)
        {
            return type.IsGenericTypeDefinition;
        }

        public static bool HasMatchingGenericArity(this Type interfaceType, Type type)
        {
            if (type.IsGenericType)
            {
                if (interfaceType.IsGenericType)
                {
                    var argumentCount = interfaceType.GetGenericArguments().Length;
                    var parameterCount = type.GetGenericArguments().Length;

                    return argumentCount == parameterCount;
                }

                return false;
            }

            return true;
        }

        public static bool HasCompatibleGenericArguments(this Type type, Type genericTypeDefinition)
        {
            var genericArguments = type.GetGenericArguments();
            try
            {
                _ = genericTypeDefinition.MakeGenericType(genericArguments);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public static Type GetRegistrationType(this Type interfaceType, Type type)
        {
            if (type.IsGenericTypeDefinition && interfaceType.IsGenericType)
            {
                return interfaceType.GetGenericTypeDefinition();
            }

            return interfaceType;
        }
    }
}