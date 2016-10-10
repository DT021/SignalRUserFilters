namespace FiltrableAlertBlotter

open System

type CompositionRoot() =
    let publisAndObsarvable() =
        let evt = Event<_>()
        evt.Trigger, evt.Publish

    let (publishStock, whenStockChanged) = publisAndObsarvable()

    static member Instance = CompositionRoot()
    member x.WhenStockChanged = whenStockChanged :> IObservable<Stock>
    member x.StockTicker = StockTicker(publishStock)