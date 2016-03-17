﻿using System;
using System.Linq.Expressions;

namespace Qactive.Expressions
{
  [Serializable]
  internal sealed class SerializableDefaultExpression : SerializableExpression
  {
    public SerializableDefaultExpression(DefaultExpression expression)
      : base(expression)
    {
    }

    internal override Expression Convert() => Expression.Default(Type);
  }
}