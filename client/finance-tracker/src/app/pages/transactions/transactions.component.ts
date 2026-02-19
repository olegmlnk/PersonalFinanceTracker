import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { BehaviorSubject, Observable, catchError, of, shareReplay, switchMap } from 'rxjs';
import { CategoriesService } from '../../core/services/categories.service';
import { TransactionsService } from '../../core/services/transactions.service';
import { Category } from '../../models/category.model';
import {
  Transaction,
  TransactionFilters,
  TransactionType,
  TransactionUpsertRequest
} from '../../models/transaction.model';
import { TransactionFormComponent } from '../../shared/components/transaction-form.component';
import { TransactionListComponent } from '../../shared/components/transaction-list.component';

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TransactionFormComponent, TransactionListComponent],
  templateUrl: './transactions.component.html',
  styleUrl: './transactions.component.scss'
})
export class TransactionsComponent {
  private readonly fb = inject(FormBuilder);
  private readonly transactionsService = inject(TransactionsService);
  private readonly categoriesService = inject(CategoriesService);
  private readonly refreshTrigger$ = new BehaviorSubject<void>(undefined);

  protected readonly typeOptions: TransactionType[] = ['income', 'expense'];
  protected readonly filterForm = this.fb.nonNullable.group({
    from: [''],
    to: [''],
    type: ['' as TransactionType | '']
  });

  protected readonly categories$: Observable<Category[]> = this.categoriesService.getCategories().pipe(
    catchError(() => of([]))
  );

  protected readonly transactions$: Observable<Transaction[]> = this.refreshTrigger$.pipe(
    switchMap(() => this.transactionsService.getTransactions(this.buildFilters())),
    catchError(() => of([])),
    shareReplay({ bufferSize: 1, refCount: true })
  );

  protected isFormOpen = false;
  protected editingTransaction: Transaction | null = null;

  protected applyFilters(): void {
    this.refreshTrigger$.next();
  }

  protected clearFilters(): void {
    this.filterForm.reset({
      from: '',
      to: '',
      type: ''
    });
    this.refreshTrigger$.next();
  }

  protected openCreateForm(): void {
    this.editingTransaction = null;
    this.isFormOpen = true;
  }

  protected onEditTransaction(transaction: Transaction): void {
    this.editingTransaction = transaction;
    this.isFormOpen = true;
  }

  protected onDeleteTransaction(transactionId: number): void {
    this.transactionsService.deleteTransaction(transactionId).subscribe({
      next: () => this.refreshTrigger$.next(),
      error: () => undefined
    });
  }

  protected onFormSubmit(payload: TransactionUpsertRequest): void {
    const request$ = this.editingTransaction
      ? this.transactionsService.updateTransaction(this.editingTransaction.id, payload)
      : this.transactionsService.createTransaction(payload);

    request$.subscribe({
      next: () => {
        this.closeForm();
        this.refreshTrigger$.next();
      },
      error: () => undefined
    });
  }

  protected closeForm(): void {
    this.editingTransaction = null;
    this.isFormOpen = false;
  }

  private buildFilters(): TransactionFilters {
    const { from, to, type } = this.filterForm.getRawValue();
    const filters: TransactionFilters = {};

    if (from) {
      filters.from = from;
    }

    if (to) {
      filters.to = to;
    }

    if (type) {
      filters.type = type;
    }

    return filters;
  }
}
