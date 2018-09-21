﻿using System;
using ValidationServices.Fluent.Internal;
using ValidationServices.Fluent.Rules;
using ValidationServices.Results;

namespace ValidationServices.Fluent.Validators.Length
{
    public static class StringLengthRuleExtension
    {
        public static PropertyValidationRule<TOwner, string> Length<TOwner>(
            this PropertyValidationRule<TOwner, string> rule, int min, int max)
        {
            rule.SetPropertyValidator(new StringLengthValidator(min, max));
            return rule;
        }

        public static PropertyValidationRule<TOwner, string> Length<TOwner>(
            this PropertyValidationRule<TOwner, string> rule, Func<TOwner, int> min, Func<TOwner, int> max)
        {
            rule.SetPropertyValidator(new StringLengthValidator(min.CoerceToNonGeneric(), max.CoerceToNonGeneric()));
            return rule;
        }
    }

    public class StringLengthValidator : IPropertyValidator
    {
        private readonly int min;
        private readonly int max;
        private readonly Func<object, int> minFunc;
        private readonly Func<object, int> maxFunc;
        protected static readonly int MAX_NOT_SPECIFIED = -1;

        public string FailureMessage { get; set; } = "Length of a string must satisfy specified constraints";

        public StringLengthValidator(int min, int max)
        {
            this.min = min;
            this.max = max;

            if (max != MAX_NOT_SPECIFIED && min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "Max should be larger than min");
            }
        }

        public StringLengthValidator(Func<object, int> minFunc, Func<object, int> maxFunc)
        {
            this.minFunc = minFunc ?? throw new ArgumentNullException(nameof(minFunc));
            this.maxFunc = maxFunc ?? throw new ArgumentNullException(nameof(maxFunc));
        }

        public ElementaryConclusion Validate(object objectToValidate, object propertyToValidate)
        {
            if (objectToValidate == null)
            {
                throw new ArgumentNullException(nameof(objectToValidate));
            }
            if (propertyToValidate == null)
            {
                return new ElementaryConclusion(isValid: false, this.FailureMessage);
            }

            int min = this.minFunc != null ? this.minFunc(objectToValidate) : this.min;
            int max = this.maxFunc != null ? this.maxFunc(objectToValidate) : this.max;

            int length = propertyToValidate.ToString().Length;

            if (length < min || (length > max && max != MAX_NOT_SPECIFIED))
            {
                return new ElementaryConclusion(isValid: false, this.FailureMessage);
            }

            return new ElementaryConclusion(isValid: true);
        }
    }
}
