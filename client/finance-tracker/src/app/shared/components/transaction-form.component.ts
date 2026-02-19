import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Category } from '../../models/category.model';
import { Transaction, TransactionType, TransactionUpsertRequest } from '../../models/transaction.model';

interface TransactionFormValue {
  date: string;
  type: TransactionType;
  categoryId: number;
  amount: number;
  note: string;
}

@Component({
  selector: 'app-transaction-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './transaction-form.component.html',
  styleUrl: './transaction-form.component.scss'
})
export class TransactionFormComponent implements OnChanges {
  private readonly fb = inject(FormBuilder);

  @Input() categories: Category[] = [];
  @Input() transaction: Transaction | null = null;

  @Output() submitted = new EventEmitter<TransactionUpsertRequest>();
  @Output() cancelled = new EventEmitter<void>();

  protected readonly typeOptions: TransactionType[] = ['income', 'expense'];
  protected filteredCategories: Category[] = [];

  protected readonly form = this.fb.nonNullable.group({
    date: [this.toDateInputValue(new Date().toISOString()), Validators.required],
    type: ['expense' as TransactionType, Validators.required],
    categoryId: [0, [Validators.required, Validators.min(1)]],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    note: ['']
  });

  constructor() {
    this.form.controls.type.valueChanges.pipe(takeUntilDestroyed()).subscribe(() => {
      this.syncFilteredCategories();
      this.form.controls.categoryId.setValue(0);
    });

    this.syncFilteredCategories();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['transaction']) {
      this.applyTransaction(this.transaction);
    }

    if (changes['categories']) {
      this.syncFilteredCategories();
    }
  }

  protected onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    this.submitted.emit({
      date: value.date,
      type: value.type,
      categoryId: value.categoryId,
      amount: value.amount,
      note: value.note.trim() || undefined
    });
  }

  protected onCancel(): void {
    this.cancelled.emit();
  }

  private applyTransaction(transaction: Transaction | null): void {
    if (!transaction) {
      this.form.reset(this.getDefaultFormValue());
      this.syncFilteredCategories();
      return;
    }

    this.form.reset({
      date: this.toDateInputValue(transaction.date),
      type: transaction.type,
      categoryId: transaction.categoryId,
      amount: transaction.amount,
      note: transaction.note ?? ''
    });
    this.syncFilteredCategories();
  }

  private syncFilteredCategories(): void {
    const selectedType = this.form.controls.type.value;
    this.filteredCategories = this.categories.filter((category) => category.type === selectedType);
  }

  private getDefaultFormValue(): TransactionFormValue {
    return {
      date: this.toDateInputValue(new Date().toISOString()),
      type: 'expense',
      categoryId: 0,
      amount: 0,
      note: ''
    };
  }

  private toDateInputValue(dateValue: string): string {
    return dateValue.slice(0, 10);
  }
}
