using DIContainer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DIContainer
{
	public class Container
	{
		private readonly IDictionary<Type, Type> types;

		public Container() => this.types = new Dictionary<Type, Type>();

		public void AddType(Type type) => AddType(type, type);

		public void AddType(Type type, Type baseType)
		{
			if(!types.ContainsKey(baseType))
				types.Add(baseType, type);
		} 

		public void AddAssembly(Assembly assembly)
		{
			var loadedTypes = assembly.GetExportedTypes()
				.Where(type => IsTypeHasExportAttribute(type)
							   || IsTypeHasImportsAttributesProperties(type)
							   || IsTypeHasImportConstructorAttribute(type))
				.ToList();

			foreach (var type in loadedTypes)
			{
				AddAttributesTypes(type);
			}
		}

		private bool IsTypeHasExportAttribute(Type type)
		{
			return type.GetCustomAttribute(typeof(ExportAttribute)) != null;
		}

		private bool IsTypeHasImportConstructorAttribute(Type type)
		{
			return type.GetCustomAttribute(typeof(ImportConstructorAttribute)) != null;
		}

		private bool IsTypeHasImportsAttributesProperties(Type type)
		{
			return type.GetProperties().Any(property => property.GetCustomAttribute(typeof(ImportAttribute)) != null);
		}
		
		private void AddAttributesTypes(Type type)
		{
			if (IsTypeHasExportAttribute(type))
			{
				var exportAttr = (ExportAttribute) type.GetCustomAttribute(typeof(ExportAttribute));
				if (exportAttr.BaseType != null)
				{
					this.AddType(type, exportAttr.BaseType);
				}
				else
				{
					this.AddType(type);
				}
			}
			else
			{
				this.AddType(type);
			}
			
		}

		public object CreateInstance(Type type) => this.CreateInstanceAndResolveDependencies(type);

		public T CreateInstance<T>() => (T)this.CreateInstanceAndResolveDependencies(typeof(T));

		private object CreateInstanceAndResolveDependencies(Type type)
		{
			if (!types.ContainsKey(type))
			{
				throw new ArgumentException($"This type {type} is not in types collection");
			}

			var instanceType = types[type];
			ConstructorInfo constructor = GetConstructor(instanceType);

			return ResolveDependencies(instanceType, constructor);
		}

		private ConstructorInfo GetConstructor(Type type)
		{
			ConstructorInfo[] constructors = type.GetConstructors();

			if (constructors.Length == 0)
			{
				throw new ArgumentException($"This type: {type} has no constructors");
			}

			return constructors.First();
		}

		private object ResolveDependencies(Type type, ConstructorInfo constructor)
		{
			var constructorParameters = constructor.GetParameters();

			var constructorParametersInstances = new List<Object>();

			foreach (var parameter in constructorParameters)
			{
				constructorParametersInstances.Add(CreateInstanceAndResolveDependencies(parameter.ParameterType));
			}

			return Activator.CreateInstance(type, constructorParametersInstances.ToArray());
		}
	}
}
