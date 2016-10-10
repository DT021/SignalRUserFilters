namespace Web

open Microsoft.AspNet.SignalR
open Microsoft.AspNet.SignalR.Hubs
open FSharp.Interop.Dynamic
open FiltrableAlertBlotter
open System
open System.Threading

type MarketState = Open | Closed

[<HubName("stockTicker")>]
type StockTickerHub(whenStockChanged, stockTicker:StockTicker) =
    inherit Hub ()

    let clients() = GlobalHost.ConnectionManager.GetHubContext<StockTickerHub>().Clients
    let broadcastStockPrice = let clients = clients() in clients.All?updateStockPrice

    do whenStockChanged |> Observable.add broadcastStockPrice
    static let marketStateLock = Object()
    static let mutable marketState = MarketState.Closed

    static let updateInterval = TimeSpan.FromMilliseconds 250.
    static let mutable timer = None

    member x.GetAllStocks() = stockTicker.GetAllStocks()
    member x.GetMarketState() = sprintf "%A" marketState
    member x.OpenMarket() =
        let openMarket() =
            timer <-
                new Timer(stockTicker.UpdateStockPricesSave, null, updateInterval, updateInterval)
                |> Some
            marketState <- MarketState.Open
            x.Clients.All?marketOpened()

        lock marketStateLock
            (fun () -> match marketState with | Open -> () | Closed -> openMarket())

    member x.CloseMarket() =
        let closeMarket() =
            match timer with |None -> () |Some t -> t.Dispose()
            marketState <- MarketState.Closed
            x.Clients.All?marketClosed()

        lock marketStateLock
            (fun () -> match marketState with | Open -> closeMarket() | Closed -> ())

    member x.Reset() =
        lock marketStateLock
            (fun () -> match marketState with
                        | Open ->
                            "Market must be closed before it can be reset."
                            |> InvalidOperationException
                            |> raise
                        | Closed ->
                            stockTicker.LoadDefaultStocks(); x.Clients.All?marketReset())







