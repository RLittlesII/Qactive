﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Qactive.Expressions
{
  [Serializable]
  internal sealed class SerializableSwitchExpression : SerializableExpression
  {
    public readonly IList<Tuple<SerializableExpression, IList<SerializableExpression>>> Cases;
    public readonly Tuple<MethodInfo, Type[]> Comparison;
    public readonly SerializableExpression DefaultBody;
    public readonly SerializableExpression SwitchValue;

    public SerializableSwitchExpression(SwitchExpression expression, SerializableExpressionConverter converter)
      : base(expression)
    {
      Contract.Requires(expression != null);
      Contract.Requires(converter != null);

      Cases = expression.Cases.Select(c => Tuple.Create(converter.TryConvert(c.Body), converter.TryConvert(c.TestValues))).ToList();
      Comparison = SerializableExpressionConverter.Convert(expression.Comparison);
      DefaultBody = converter.TryConvert(expression.DefaultBody);
      SwitchValue = converter.TryConvert(expression.SwitchValue);
    }

    internal override Expression Convert() => Expression.Switch(
                                                Type,
                                                SwitchValue.TryConvert(),
                                                DefaultBody.TryConvert(),
                                                SerializableExpressionConverter.Convert(Comparison),
                                                Cases.Select(c => Expression.SwitchCase(c.Item1.TryConvert(), c.Item2.TryConvert())));
  }
}