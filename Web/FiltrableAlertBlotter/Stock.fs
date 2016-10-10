namespace FiltrableAlertBlotter

type Stock =
    {
        Symbol: string
        Price: decimal
        DayOpen: decimal
        DayLow: decimal
        DayHigh: decimal
        LastChange: decimal
        Change: decimal
        PercentChange: double
    }

module StockModule =

    let init symbol price =
        {   Symbol = symbol; Price = price; DayOpen = price
            DayLow = price; DayHigh = price; LastChange = 0m
            Change = 0m; PercentChange = 0.0
        }

    let updateDay binary previous price =
        match binary price previous with
        |true -> price |false -> previous

    let updateDayLow = updateDay (<)

    let updateDayHigh = updateDay (>)
    let round (value:decimal) = System.Math.Round(value, 4)
    let updateStock (previousStock:Stock) price =
        let lastChange = price - previousStock.Price
        let dayLow = updateDayLow previousStock.DayLow price
        let dayHigh = updateDayHigh previousStock.DayHigh price
        let change = price - previousStock.DayOpen
        let percentChange = change / price|> round
        let updatedStock = {
            previousStock with
                    Price = price
                    LastChange = lastChange
                    DayLow = dayLow
                    DayHigh = dayHigh
                    Change = change
                    PercentChange = (double) percentChange}
        updatedStock

