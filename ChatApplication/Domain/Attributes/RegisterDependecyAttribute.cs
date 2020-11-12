using System;

namespace ChatApplication.Domain.Attributes {
    public class ScopeDependecyAttribute : Attribute { }
    public class TransientDependecyAttribute : Attribute { }
    public class SingletonDependecyAttribute : Attribute { }
}