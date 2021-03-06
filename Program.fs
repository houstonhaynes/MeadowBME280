open System
open System.Threading
open Meadow.Devices
open Meadow
open Meadow.Foundation
open Meadow.Foundation.Leds
open Meadow.Foundation.Sensors.Atmospheric
open Meadow.Foundation.Graphics
open Meadow.Foundation.Displays.TftSpi


type MeadowApp() =
    inherit App<F7Micro, MeadowApp>()

    do Console.WriteLine "Let's get started!"

    // set up display
    let display = new Gc9a01 (MeadowApp.Device, 
                                MeadowApp.Device.CreateSpiBus(48000L),  
                                MeadowApp.Device.Pins.D02,  
                                MeadowApp.Device.Pins.D01,  
                                MeadowApp.Device.Pins.D00)

    let graphics = GraphicsLibrary(display)

    let displayWidth = Convert.ToInt32(display.Width)
    let displayHeight = Convert.ToInt32(display.Height)
    let originX = displayWidth / 2
    let originY = displayHeight / 2

    let loadScreen (firstColor: Color) (secondColor: Color) = 
        Console.WriteLine "Clearing Screen..."
        graphics.Clear()
        Console.WriteLine "Screen Cleared..."
        graphics.CurrentFont <- Font12x20()
        graphics.DrawCircle(originX, originY, 125, firstColor, true, true)
        graphics.DrawCircle(originX, originY, 108, Color.Black, true, true)
        graphics.DrawCircle(originX, originY, 92, secondColor, true, true)
        graphics.DrawCircle(originX, originY, 76, Color.Black, true, true)
        graphics.DrawRoundedRectangle(7, 98, 225, 44, 8, Color.Black, true)
        graphics.DrawText(11, 102, "Breathe", Color.CornflowerBlue, GraphicsLibrary.ScaleFactor.X2)
        graphics.DrawText(179, 102, "EZ", Color.HotPink, GraphicsLibrary.ScaleFactor.X2)
        Console.WriteLine "Loading Screen..."
        graphics.Show()

    do loadScreen Color.Green Color.Yellow
    do loadScreen Color.Yellow Color.Red
    do loadScreen Color.Red Color.Red

    // set up sensor
    //let i2c = MeadowApp.Device.CreateI2cBus(Hardware.I2cBusSpeed.Fast)
    //let sensor = Bme280 (i2c, Bme280.I2cAddress.Adddress0x76)
    //let consumer = Bme280.CreateObserver(fun result -> printfn $"Reading is {result.New}")

    // <Units.Temperature.UnitType.Fahrenheit, Units.Pressure.UnitType.Millibar, Units.RelativeHumidity.UnitType.Percent>

    //let s = sensor.Subscribe(consumer)

    //do sensor.StartUpdating(TimeSpan.FromSeconds(2.0))

    
    // boilerplate LED stuff
    let led =
        RgbPwmLed(MeadowApp.Device, MeadowApp.Device.Pins.OnboardLedRed, MeadowApp.Device.Pins.OnboardLedGreen,
                      MeadowApp.Device.Pins.OnboardLedBlue, 3.3f, 3.3f, 3.3f,
                      Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode)

    let showColorPulses color duration =
        led.StartPulse(color, (duration / 2)) |> ignore
        Threading.Thread.Sleep(int duration) |> ignore
        led.Stop |> ignore

    let cycleColors duration =
        while true do
            showColorPulses Color.Blue duration
            showColorPulses Color.Cyan duration
            showColorPulses Color.Green duration
            showColorPulses Color.GreenYellow duration
            showColorPulses Color.Yellow duration
            showColorPulses Color.Orange duration
            showColorPulses Color.OrangeRed duration
            showColorPulses Color.Red duration
            showColorPulses Color.MediumVioletRed duration
            showColorPulses Color.Purple duration
            showColorPulses Color.Magenta duration
            showColorPulses Color.Pink duration

    do cycleColors 2000

[<EntryPoint>]
let main argv =
    Console.WriteLine "Hello World from F#!"
    let app = MeadowApp()
    Threading.Thread.Sleep(System.Threading.Timeout.Infinite)
    0 // return an integer exit code