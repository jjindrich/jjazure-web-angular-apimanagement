import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import {Book} from '../book';
import {BOOKS} from '../mock-books';

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.css']
})
export class BooksComponent implements OnInit { 
  //books = BOOKS;
  books: Book[];
  selectedBook: Book;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Book[]>(baseUrl + 'api/books').subscribe(result => {
      this.books = result;
      console.log("retrieved" + this.books);
      console.log("title: " + this.books[0].name);
    }, error => console.error(error));
  }

  ngOnInit() {
  }

  onSelect(book: Book): void {
    this.selectedBook = book;
  }
}
