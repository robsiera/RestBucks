﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Infrastructure.HyperMedia.Linker.Interfaces;

namespace Infrastructure.HyperMedia.Linker
{
    /// <summary>
    /// The default Strategy for dispatching Action Methods to a route name, by
    /// always dispatching to a single, named route.
    /// </summary>
    /// <seealso cref="IRouteDispatcher" />
    public class DefaultRouteDispatcher : IRouteDispatcher
    {
        private readonly string _routeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRouteDispatcher" /> class.
        /// </summary>
        public DefaultRouteDispatcher()
            : this("DefaultApi")
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DefaultRouteDispatcher" /> class with the supplied route
        /// name.
        /// </summary>
        /// <param name="routeName">
        /// The route name which will be used by the
        /// <see cref="Dispatch(MethodCallExpression, IDictionary{string, object})" />
        /// method as the <see cref="Rouple.RouteName" /> value.
        /// </param>
        /// <remarks>
        /// <para>
        /// After initialization, the <paramref name="routeName" /> value is
        /// available through the <see cref="RouteName" /> property.
        /// </para>
        /// </remarks>
        public DefaultRouteDispatcher(string routeName)
        {
            if (routeName == null)
                throw new ArgumentNullException("routeName");

            _routeName = routeName;
        }

        /// <summary>
        /// Provides dispatch information based on an Action Method.
        /// </summary>
        /// <param name="method">The method expression.</param>
        /// <param name="routeValues">Route values.</param>
        /// <returns>
        /// An object containing the route name, as well as the route values.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">method</exception>
        /// <remarks>
        /// <para>
        /// The returned <see cref="Rouple.RouteName" /> will be the value of
        /// the <see cref="RouteName" /> property.
        /// </para>
        /// <para>
        /// The returned <see cref="Rouple.RouteValues" /> will be all entries
        /// of the <paramref name="routeValues" />, plus a value for an
        /// additional "controller" key, derived from
        /// <paramref name="method" />.
        /// </para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "This method should produce URIs with lower-case letters, so ultimately, it would have to invoke some sort of ToLower method.")]
        public Rouple Dispatch(
            MethodCallExpression method,
            IDictionary<string, object> routeValues)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            var newRouteValues = new Dictionary<string, object>(routeValues);

           if (method
                  .Object != null)
           {
              var controllerName = method
                 .Object
                 .Type
                 .Name
                 .ToLowerInvariant()
                 .Replace("controller", "");
              newRouteValues["controller"] = controllerName;
           }

           return new Rouple(_routeName, newRouteValues);
        }

        /// <summary>
        /// Gets the route name.
        /// </summary>
        /// <seealso cref="DefaultRouteDispatcher(string)" />
        public string RouteName
        {
            get { return _routeName; }
        }
    }
}
