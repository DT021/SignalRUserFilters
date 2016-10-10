module StockTickerTests

open NUnit.Framework
open FiltrableAlertBlotter.StockTickerModule
open Swensen.Unquote

[<Test>]
let ``can init`` () =
    let st = init()
    st.Count > 0 =! true

[<Test>]
let ``stocks map to list`` () =
    let st = init()
    let stl = st |> stocksList
    stl.Length > 0 =! true

[<TestCase(100000)>]
let ``once per rolls stock should be updated`` rolls =
    let re = [0..rolls] |> Seq.map(fun _ -> simulateUpdateOrNot() ) |> Seq.find(fun r -> r)
    re =! true

[<Test>]
let ``stock change is not zero`` () =
    let re = simulateStockChange (init().["MSFT"].Price)
    re <> 0.m =! true

[<TestCase(10000)>]
let ``update stock prices, at least one price changed`` rolls =
    let st = init()
    let pubMock st = ()
    let re =
        [0..rolls] |> Seq.map(fun _ ->
            let ust = st |> updateStockPrices pubMock
            ust.["MSFT"].Price <> st.["MSFT"].Price
            || ust.["AAPL"].Price <> st.["AAPL"].Price
            || ust.["GOOG"].Price <> st.["GOOG"].Price
            ) |> Seq.find(fun r -> r)
    re =! true