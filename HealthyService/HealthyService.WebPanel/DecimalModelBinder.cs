using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HealthyService.WebPanel
{
    public class DecimalModelBinder : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (!context.Metadata.IsComplexType && (context.Metadata.ModelType == typeof(decimal) || context.Metadata.ModelType == typeof(decimal?)))
            {
                return new InvariantDecimalModelBinder(context.Metadata.ModelType, (ILoggerFactory)context.Services.GetService(typeof(ILoggerFactory)));
            }

            return null;
        }

        public class InvariantDecimalModelBinder : IModelBinder
        {
            private readonly SimpleTypeModelBinder _baseBinder;

            public InvariantDecimalModelBinder(Type modelType, ILoggerFactory loggerFactory)
            {
                _baseBinder = new SimpleTypeModelBinder(modelType, loggerFactory);
            }

            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

                var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                if (valueProviderResult != ValueProviderResult.None)
                {
                    bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

                    var valueAsString = valueProviderResult.FirstValue;
                    decimal result;

                    // Use invariant culture
                    if (decimal.TryParse(valueAsString, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result))
                    {
                        bindingContext.Result = ModelBindingResult.Success(result);
                        return Task.CompletedTask;
                    }
                }

                // If we haven't handled it, then we'll let the base SimpleTypeModelBinder handle it
                return _baseBinder.BindModelAsync(bindingContext);
            }
        }
    }
}
