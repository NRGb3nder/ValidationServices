﻿using System.Reflection;
using Xunit;
using ValidationService.Attributes;
using ValidationServiceTests.TestEntities;
using System;

namespace ValidationServiceTests {
    public class StringLengthConstraintAttributeTest {
        private readonly StringLengthConstraintFoobar obj = new StringLengthConstraintFoobar();

        [Fact]
        public void NullStringsAreValid() {
            StringLengthConstraintAttribute attr = 
                (StringLengthConstraintAttribute)GetStringLengthConstraintAttribute(nameof(this.obj.Ok1));
            Assert.True(attr.Validate(this.obj.Ok1).IsValid);
        }

        [Fact]
        public void MinMaxConstraintedStringIsOk() {
            StringLengthConstraintAttribute attr =
               (StringLengthConstraintAttribute)GetStringLengthConstraintAttribute(nameof(this.obj.Ok2));
            Assert.True(attr.Validate(this.obj.Ok2).IsValid);
        }

        [Fact]
        public void MinMaxLowerConstraintViolationIsNotOk() {
            StringLengthConstraintAttribute attr =
               (StringLengthConstraintAttribute)GetStringLengthConstraintAttribute(nameof(this.obj.ShortNotOk2));
            Assert.False(attr.Validate(this.obj.ShortNotOk2).IsValid);
        }

        [Fact]
        public void MinMaxUpperConstraintViolationIsNotOk() {
            StringLengthConstraintAttribute attr =
               (StringLengthConstraintAttribute)GetStringLengthConstraintAttribute(nameof(this.obj.LongNotOk2));
            Assert.False(attr.Validate(this.obj.LongNotOk2).IsValid);
        }

        [Fact]
        public void InvalidStringHasCorrespondingFailureMessage() {
            StringLengthConstraintAttribute attr =
                (StringLengthConstraintAttribute)GetStringLengthConstraintAttribute(nameof(this.obj.ShortNotOk2));
            Assert.Equal(attr.FailureMessage, attr.Validate(this.obj.ShortNotOk2).Details);
        }

        [Fact]
        public void MaxConstraintedStringIsOk() {
            StringLengthConstraintAttribute attr =
                (StringLengthConstraintAttribute)GetStringLengthConstraintAttribute(nameof(this.obj.Ok3));
            Assert.True(attr.Validate(this.obj.Ok3).IsValid);
        }

        [Fact]
        public void MaxConstraintViolationIsNotOk() {
            StringLengthConstraintAttribute attr =
                (StringLengthConstraintAttribute)GetStringLengthConstraintAttribute(nameof(this.obj.NotOk3));
            Assert.False(attr.Validate(this.obj.NotOk3).IsValid);
        }

        [Fact]
        public void MinConstraintedStringIsOk() {
            StringLengthConstraintAttribute attr =
                (StringLengthConstraintAttribute)GetStringLengthConstraintAttribute(nameof(this.obj.Ok4));
            Assert.True(attr.Validate(this.obj.Ok4).IsValid);
        }

        [Fact]
        public void MinConstraintViolationIsNotOk() {
            StringLengthConstraintAttribute attr =
                (StringLengthConstraintAttribute)GetStringLengthConstraintAttribute(nameof(this.obj.NotOk4));
            Assert.False(attr.Validate(this.obj.NotOk4).IsValid);
        }

        [Fact]
        public void NotStringIsNotAllowed() {
            StringLengthConstraintAttribute attr =
                (StringLengthConstraintAttribute)GetStringLengthConstraintAttribute(nameof(this.obj.NotOk5));
            Assert.Throws<ArgumentException>(() => { attr.Validate(this.obj.NotOk5); });
        }
        
        [Fact]
        public void LowerConstraintCanNotExceedUpperConstraint() {
            StringLengthConstraintAttribute attr =
                (StringLengthConstraintAttribute)GetStringLengthConstraintAttribute(nameof(this.obj.NotOk6));
            Assert.Throws<ArgumentException>(() => { attr.Validate(this.obj.NotOk6); });
        }

        [Fact]
        public void AtLeastOneConstraintMustBeSpecified() {
            StringLengthConstraintAttribute attr =
                (StringLengthConstraintAttribute)GetStringLengthConstraintAttribute(nameof(this.obj.NotOk7));
            Assert.Throws<ArgumentException>(() => { attr.Validate(this.obj.NotOk7); });
        }

        private static ValidationAttribute GetStringLengthConstraintAttribute(string propName) {
            PropertyInfo info = typeof(StringLengthConstraintFoobar).GetProperty(propName);

            return (StringLengthConstraintAttribute)info.GetCustomAttribute(typeof(StringLengthConstraintAttribute));
        }
    }
}
