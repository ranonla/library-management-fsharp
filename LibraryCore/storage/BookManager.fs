namespace LibraryCore

open System
open Microsoft.Data.Sqlite
open LibraryCore.BookModel

module BookManager =

    let connectionString = "Data Source=app.db"

    let useConnection (f: SqliteConnection -> 'T) =
        use conn = new SqliteConnection(connectionString)
        conn.Open()
        f conn

    let createTable () =
        useConnection (fun conn ->
            let sql = """
            CREATE TABLE IF NOT EXISTS Books (
                Id TEXT PRIMARY KEY,
                Title TEXT NOT NULL,
                Author TEXT NOT NULL,
                Status TEXT NOT NULL CHECK (Status IN ('Available', 'Borrowed'))
            )
            """
            use cmd = new SqliteCommand(sql, conn)
            cmd.ExecuteNonQuery() |> ignore
        )

    let private statusToString = function
        | BookStatus.Available -> "Available"
        | BookStatus.Borrowed -> "Borrowed"

    let private stringToStatus = function
        | "Available" -> BookStatus.Available
        | "Borrowed" -> BookStatus.Borrowed
        | _ -> BookStatus.Available

    let insertBook (title: string) (author: string) =
        let book = createBook title author
        useConnection (fun conn ->
            let sql = "INSERT INTO Books (Id, Title, Author, Status) VALUES ($id, $title, $author, $status)"
            use cmd = new SqliteCommand(sql, conn)

            cmd.Parameters.AddWithValue("$id", book.Id.ToString()) |> ignore
            cmd.Parameters.AddWithValue("$title", book.Title) |> ignore
            cmd.Parameters.AddWithValue("$author", book.Author) |> ignore
            cmd.Parameters.AddWithValue("$status", statusToString book.Status) |> ignore

            cmd.ExecuteNonQuery() |> ignore
            book
        )

    let getAllBooks () : Book list =
        useConnection (fun conn ->
            let sql = "SELECT Id, Title, Author, Status FROM Books"
            use cmd = new SqliteCommand(sql, conn)
            use reader = cmd.ExecuteReader()

            let list = System.Collections.Generic.List<Book>()

            while reader.Read() do
                list.Add {
                    Id = Guid.Parse(reader.GetString(0))
                    Title = reader.GetString(1)
                    Author = reader.GetString(2)
                    Status = stringToStatus (reader.GetString(3))
                }

            list |> Seq.toList
        )

    let searchBooks (query: string) : Book list =
        let books = getAllBooks()
        let q = query.ToLower()

        match Guid.TryParse(query) with
        | true, guid ->
            books |> List.filter (fun b -> b.Id = guid)
        | _ ->
            books
            |> List.filter (fun b ->
                b.Title.ToLower().StartsWith(q) ||
                b.Author.ToLower().StartsWith(q)
        )

    let updateBookTitle (id: Guid) (newTitle: string) =
        useConnection (fun conn ->
            let sql = "UPDATE Books SET Title = $title WHERE Id = $id"
            use cmd = new SqliteCommand(sql, conn)
            cmd.Parameters.AddWithValue("$title", newTitle) |> ignore
            cmd.Parameters.AddWithValue("$id", id.ToString()) |> ignore
            cmd.ExecuteNonQuery() |> ignore
        )

    let updateAuthor (id: Guid) (newAuthor: string) =
        useConnection (fun conn ->
            let sql = "UPDATE Books SET Author = $author WHERE Id = $id"
            use cmd = new SqliteCommand(sql, conn)
            cmd.Parameters.AddWithValue("$author", newAuthor) |> ignore
            cmd.Parameters.AddWithValue("$id", id.ToString()) |> ignore
            cmd.ExecuteNonQuery() |> ignore
        )

    let updateStatus (id: Guid) (status: BookStatus) =
        useConnection (fun conn ->
            let sql = "UPDATE Books SET Status = $status WHERE Id = $id"
            use cmd = new SqliteCommand(sql, conn)
            cmd.Parameters.AddWithValue("$status", statusToString status) |> ignore
            cmd.Parameters.AddWithValue("$id", id.ToString()) |> ignore
            cmd.ExecuteNonQuery() |> ignore
        )

    let deleteBook (id: Guid) =
        useConnection (fun conn ->
            let sql = "DELETE FROM Books WHERE Id = $id"
            use cmd = new SqliteCommand(sql, conn)
            cmd.Parameters.AddWithValue("$id", id.ToString()) |> ignore
            cmd.ExecuteNonQuery() |> ignore
        )

    let tryFindBook (query: string) : Book option =
        let books = getAllBooks()

        match Guid.TryParse(query) with
        | true, guid ->
            books |> List.tryFind (fun b -> b.Id = guid)
        | _ ->
            let q = query.ToLower()
            books
            |> List.tryFind (fun b ->
                b.Title.ToLower().StartsWith(q)
        )

    let deleteBookByQuery (query: string) : string option =
        match tryFindBook query with
        | Some book ->
            deleteBook book.Id
            Some "Book deleted successfully!"
        | None ->
            Some "Book not found."

    let borrowBook (id: Guid) : string option =
        match tryFindBook (id.ToString()) with
        | None -> Some "Book not found."
        | Some book ->
            match book.Status with
            | Borrowed -> Some "Book is already borrowed."
            | Available ->
                updateStatus id BookStatus.Borrowed
                Some "Book borrowed successfully!"

    let borrowBookByQuery (query: string) : string option =
        match tryFindBook query with
        | Some book -> borrowBook book.Id
        | None -> Some "Book not found."

    let returnBook (id: Guid) : string option =
        match tryFindBook (id.ToString()) with
        | None -> Some "Book not found."
        | Some book ->
            match book.Status with
            | Available -> Some "Book is already available."
            | Borrowed ->
                updateStatus id BookStatus.Available
                Some "Book returned successfully!"

    let returnBookByQuery (query: string) : string option =
        match tryFindBook query with
        | Some book -> returnBook book.Id
        | None -> Some "Book not found."

