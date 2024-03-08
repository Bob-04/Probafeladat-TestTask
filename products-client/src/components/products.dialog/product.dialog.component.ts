import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Product } from '../../models/product';
import { MatFormField, MatFormFieldModule } from '@angular/material/form-field';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-product-dialog',
  standalone: true,
  templateUrl: './product.dialog.component.html',
  styleUrls: ['./product.dialog.component.css'],
  imports: [MatFormField, FormsModule, CommonModule, MatInputModule, ReactiveFormsModule, MatButtonModule]
})
export class ProductDialogComponent implements OnInit {
  productForm: FormGroup = this.fb.group({
    name: ['', Validators.required], // Name field with required validation
    price: ['', [Validators.required, Validators.min(0)]], // Price field with required and minimum value validation
    description: ['', Validators.required] // Description field
  });

  isNew: boolean = true;
  nameError: boolean = false;
  priceError: boolean = false;
  descriptionError: boolean = false;

  constructor(
    public dialogRef: MatDialogRef<ProductDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    if (this.data && !this.data.isNew) {
      this.productForm.patchValue(this.data); // Pre-populate form with data if editing
    }
    this.isNew = this.data?.isNew ?? true;
  }

  onSave(): void {
    this.nameError = this.productForm?.get('name')?.valid ?? false;
    this.priceError = this.productForm?.get('price')?.valid ?? false;
    this.descriptionError = this.productForm?.get('description')?.valid ?? false;
    if (this.productForm?.valid) {
      if (this.data.id) {
        this.productForm.value.id = this.data.id
      }
      this.dialogRef.close(this.productForm.value);
    }
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }
}
