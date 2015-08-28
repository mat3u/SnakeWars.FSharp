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

        // -- Sample
        let me = game.Me(snakeId)

        let isTaken (pos: State.Cell) =
            game.Taken |> Seq.exists (fun e -> e == pos)

        let possible = [GoStraight; TurnLeft; TurnRight]
                       |> Seq.map (fun m -> (m, game.NextHeadPosition snakeId m))
                       |> Seq.where (fun (m, p) -> not (isTaken p))
                       |> Seq.map fst
                       |> Seq.toList

        match possible with
        | f::_ -> f
        | _ -> GoStraight

        // --- ---

    while not System.Console.KeyAvailable do
        try
            game host port login snake
        with
        | e -> printf "Error: %A\r\nRestarting connection...!" e

    0
