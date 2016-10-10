module StockTests

open NUnit.Framework
open FiltrableAlertBlotter.StockModule
open Swensen.Unquote

[<TestCase("MSFT", 41.68)>]
let ``can craete test stock type`` symbol price =
    let stock = init symbol price
    stock =! {Symbol=symbol; Price=price; DayOpen=price;
                DayLow=price; DayHigh=price; LastChange=0m;
                Change=0m; PercentChange=0.0}

[<TestCase(42.01, 0.12, 41.89, 42.01, 0.12, 0.0029)>]
[<TestCase(41.01, -0.88, 41.01, 41.89, -0.88, -0.0215)>]
let ``update stock`` price lastChange dayLow dayHigh change
    percentChange=
    let stock = init "MSFT" 41.89m
    let stock' = updateStock stock price
    stock' =! {stock with Price = price; LastChange = lastChange;
                            DayLow = dayLow; DayHigh = dayHigh;
                            Change = change; PercentChange = percentChange}