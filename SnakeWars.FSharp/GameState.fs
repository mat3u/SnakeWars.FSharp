module GameState

open FSharp.Data

type State = JsonProvider<"SampleGameState.json">
type Direction = Left | Right | Straight

let decode raw =
    async {
        let! json = raw
        return State.Parse(json)
    }
