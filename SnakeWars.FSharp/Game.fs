module Game

open System.IO
open System.Net
open System.Net.Sockets

open GameState

let game host port (login: string) bot =
    use tcp = new TcpClient(host, port)
    use sw = new StreamWriter(tcp.GetStream())
    use sr = new StreamReader(tcp.GetStream())

    sw.AutoFlush <- true

    match sr.ReadLine().Trim() with
    | "ID" -> sw.WriteLine(login)
    | a -> failwith "Server didn't ask for identity!"

    let snakeId = sr.ReadLine()

    sr.ReadLine() |> ignore

    let loop() =
        let rec loop'() =
            async {
                let! state = sr.ReadLineAsync() |> Async.AwaitTask |> decode

                let direction = bot state snakeId

                printf "%A\n" direction

                match direction with
                | TurnLeft -> sw.WriteLine("LEFT")
                | TurnRight -> sw.WriteLine("RIGHT")
                | _ -> sw.WriteLine("NONE")

                return! loop'()
            }

        loop'()

    loop() |> Async.RunSynchronously

    ()
