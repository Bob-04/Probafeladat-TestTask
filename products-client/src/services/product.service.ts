import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { environment } from '../environments/environment.development';
import { ProductCreate } from '../models/product.create';
import { Product } from '../models/product';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private productsSubject = new BehaviorSubject<Product[]>([]);
  products$: Observable<Product[]> = this.productsSubject.asObservable();
  private apiUrl = environment.apiUrl + 'products';

  constructor(private httpClient: HttpClient,
    private toastr: ToastrService) { }

  getProducts(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(this.apiUrl)
      .pipe(tap(products => this.productsSubject.next(products)));
  }

  getProduct(id: string): Observable<Product> {
    return this.httpClient.get<Product>(this.apiUrl + '/' + id)
      .pipe(tap(product => { this.productsSubject.next([...this.productsSubject.getValue(), product]); }));
  }

  addProduct(product: ProductCreate): any {
    return this.httpClient.post<Product>(this.apiUrl, product)
      .pipe(
        tap(_ => this.getProducts()),
        catchError(async (error) => this.handleError(error))
      );
  }

  editProduct(product: Product): any {
    return this.httpClient.put<Product>(this.apiUrl, product)
      .pipe(
        tap(_ => this.getProducts()),
        catchError(async (error) => this.handleError(error))
      );
  }

  deleteProduct(id: string): any {
    return this.httpClient.delete(this.apiUrl + '/' + id)
      .pipe(
        tap(_ => this.getProducts()),
        catchError(async (error) => this.handleError(error))
      );
  }

  private handleError(error: any): void {
    console.error('An error occurred:', error);
    this.toastr.error('An error occurred. Please try again later. ' + (error ? '\nError: ' + error.error.message : ''));
  }
}
