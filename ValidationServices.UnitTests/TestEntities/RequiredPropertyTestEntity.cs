﻿using System.Collections.Generic;
using ValidationServices.Attributes;

namespace ValidationServices.UnitTests.TestEntities
{
    public class RequiredPropertyTestEntity
    {
        [RequiredProperty]
        public object Ok1 { get; } = new List<int>();
        [RequiredProperty]
        public object NotOk1 { get; }

        [RequiredProperty(AllowEmptyStrings = true)]
        public string Ok2 { get; } = "";
        [RequiredProperty]
        public string NotOk2 { get; } = "";

        [RequiredProperty(AllowEmptyStrings = true)]
        public string Ok3 { get; } = " ";
        [RequiredProperty]
        public string NotOk3 { get; } = " ";

        [RequiredProperty]
        public string Ok4 { get; } = "a string";
        [RequiredProperty]
        public string NotOk4 { get; }
    }
}
