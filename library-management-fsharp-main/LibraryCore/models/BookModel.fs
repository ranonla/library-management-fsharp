namespace LibraryCore

module BookModel =
    open System

    type BookStatus =
        | Available
        | Borrowed

    type Book = {
        Id: Guid
        Title: string
        Author: string
        Status: BookStatus
    }    

    let createBook title author =
        {
            Id = Guid.NewGuid()
            Title = title
            Author = author
            Status = Available
        }