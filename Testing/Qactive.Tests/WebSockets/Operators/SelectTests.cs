﻿using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Qactive.Tests.WebSockets.Operators
{
  [TestClass]
  public class SelectTests : TestBase
  {
    [TestMethod]
    public async Task Select()
    {
      var service = WebSocketTestService.Create(Observable.Range(0, 5));
      var results = await service.QueryAsync(xs => from x in xs
                                                   select x * 5);

      QactiveAssert.AreEqual(results, OnNext(0), OnNext(5), OnNext(10), OnNext(15), OnNext(20), OnCompleted<int>());
    }

    [TestMethod]
    public async Task SelectClosure()
    {
      Func<int, int> selector = x => x * 5;

      var service = WebSocketTestService.Create(WebSocketTestService.UnrestrictedOptions, Observable.Range(0, 5));
      var results = await service.QueryAsync(xs => from x in xs
                                                   select selector(x));

      QactiveAssert.AreEqual(results, OnNext(0), OnNext(5), OnNext(10), OnNext(15), OnNext(20), OnCompleted<int>());
    }
  }
}
