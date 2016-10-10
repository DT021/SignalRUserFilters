namespace Web

open FiltrableAlertBlotter
open Microsoft.AspNet.SignalR
open Owin

type Startup() =
    static member ConfigureSignalR(app: IAppBuilder) =
        let cr = CompositionRoot.Instance
        GlobalHost.DependencyResolver.Register(typeof<StockTickerHub>,
                fun () -> new StockTickerHub(cr.WhenStockChanged, cr.StockTicker))
        app.MapSignalR()



