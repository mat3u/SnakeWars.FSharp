open GameState
open Game

[<EntryPoint>]
let main argv =
    // 1. Specify your connection settings
    let login = "PL2"
    let host = "localhost"
    let port = 9977

    let snake (game : State.Root) snakeId : Direction =
        // 2. Implement snake logic here!
        Left

        // --- ---

    while not System.Console.KeyAvailable do
        try
            game host port login snake
        with
        | e -> printf "Error: %A\r\nRestarting connection...!" e

    0
