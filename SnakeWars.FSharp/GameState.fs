module GameState

open FSharp.Data

type State = JsonProvider<"SampleGameState.json">
type Direction = TurnLeft | TurnRight | GoStraight

let decode raw =
    async {
        let! json = raw
        return State.Parse(json)
    }

type State.Root with
    member this.Me snakeId =
        this.Snakes |> Seq.where (fun s -> s.Id = snakeId) |> Seq.exactlyOne
    member this.NextHeadPosition snakeId move =
        let me = this.Me(snakeId)

        let x = me.Head.X
        let y = me.Head.Y

        let hd ox oy =
            let nx = (me.Head.X + ox + this.BoardSize.Width) % this.BoardSize.Width
            let ny = (me.Head.Y + oy + this.BoardSize.Height) % this.BoardSize.Height

            new State.Cell(nx, ny)

        match (me.Direction, move) with
        | ("Up", TurnLeft) -> hd -1 0
        | ("Up", GoStraight) -> hd 0 1
        | ("Up", TurnRight) -> hd 1 0

        | ("Down", TurnLeft) -> hd 1 0
        | ("Down", GoStraight) -> hd 0 -1
        | ("Down", TurnRight) -> hd -1 0

        | ("Left", TurnLeft) -> hd 0 -1
        | ("Left", GoStraight) -> hd -1 0
        | ("Left", TurnRight) -> hd 0 1

        | ("Right", TurnLeft) -> hd 0 1
        | ("Right", GoStraight) -> hd 1 0
        | ("Right", TurnRight) -> hd 0 -1

        | ("None", _) -> hd 0 0
        | _ -> failwith "Unknown head position!"
    member this.Taken with get () : State.Cell seq =
        this.Snakes
        |> Seq.collect (fun s -> s.Cells)
        |> Seq.append this.Walls

let (==) (a: State.Cell) (b: State.Cell) =
    a.X = b.X && a.Y = b.Y
