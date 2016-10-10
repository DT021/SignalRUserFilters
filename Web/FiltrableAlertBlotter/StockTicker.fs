namespace FiltrableAlertBlotter

open StockModule

module StockTickerModule =
    open System

    let init() = Map.empty |> Map.add "MSFT" (init "MSFT" 41.68m)
                    |> Map.add "AAPL" (init "AAPL" 92.08m)
                    |> Map.add "GOOG" (init "GOOG" 543.01m)

    let stocksList stocks = stocks |> Map.toList |> List.map snd

    let simulateUpdateOrNot() =
        let rnd = Random()
        rnd.NextDouble() > 0.1

    let simulateStockChange (price:decimal) =
        let fl = Math.Floor(price)
        let rnd1 = Random((int)fl)
        let percentChange = rnd1.NextDouble() * 0.002
        let negative = rnd1.NextDouble() > 0.51
        let change = Math.Round(price * (decimal) percentChange, 2)
        let re = if negative then change * -1m else change
        re

    let updateStockPrices publishStock stocks =
        let re = stocks
                    |> Map.map(fun _ v -> if simulateUpdateOrNot() then
                                            let change = simulateStockChange v.Price
                                            let newPrice = v.Price + change
                                            let us = updateStock v newPrice
                                            publishStock us
                                            us
                                          else v)
        re

    let _lock = Object()
    let updateStockPricesSave publishStock stocks =
        let mutable newStocks = stocks
        lock _lock (fun () -> newStocks <- updateStockPrices publishStock stocks )
        newStocks

open StockTickerModule

type StockTicker (publishStock) =
    let mutable stocks = init()
    member x.LoadDefaultStocks() = stocks <- init()
    member x.GetAllStocks() = stocks |> stocksList
    member x.UpdateStockPricesSave state = stocks <- updateStockPrices publishStock stocks

