import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Product } from '../../models/product';
import { ProductService } from '../../services/product.service';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { ProductDialogComponent } from '../products.dialog/product.dialog.component';
import { MatTableModule } from '@angular/material/table';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';

@Component({
  standalone: true,
  selector: 'products',
  templateUrl: './products.component.html',
  imports: [MatIconModule, CommonModule, MatTableModule, HttpClientModule, MatButtonModule],
})
export class ProductsComponent implements OnInit {
  products: Product[] = [];
  error: string = '';
  displayedColumns: string[] = ['name', 'price', 'description', 'actions'];

  constructor(
    private productService: ProductService,
    public dialog: MatDialog,
    private changeDetectorRefs: ChangeDetectorRef
  ) {
    this.productService.getProducts().subscribe();

  }

  ngOnInit(): void {
    this.productService.products$.subscribe(products => {
      this.products = products;
    });
  }

  openAddProductDialog(): void {
    const dialogRef = this.dialog.open(ProductDialogComponent, {
      data: { isNew: true }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.productService.addProduct(result).subscribe(
          () => {
            this.refresh();
          }
        )
      }
    });
  }

  openEditProductDialog(product: Product): void {
    const dialogRef = this.dialog.open(ProductDialogComponent, {
      data: { ...product, isNew: false }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.productService.editProduct(result).subscribe(
          () => {
            this.refresh();
          }
        );
      }
    });
  }

  deleteProduct(id: string): void {
    this.productService.deleteProduct(id).subscribe(
      () => {
        this.refresh();
      }
    );
  }

  refresh(): void {
    this.productService.getProducts().subscribe(() => this.changeDetectorRefs.detectChanges());
  }
}
