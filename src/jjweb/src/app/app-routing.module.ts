import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BooksComponent } from './books/books.component';

import { NotFoundComponent } from './not-found/not-found.component';
import { AdalGuard } from 'adal-angular4';

const routes: Routes = [
  { path: 'books', component: BooksComponent, canActivate: [AdalGuard] },
  { path: '**', component: NotFoundComponent },
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
