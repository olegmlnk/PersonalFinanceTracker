import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Transaction } from '../../models/transaction.model';

@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './transaction-list.component.html',
  styleUrl: './transaction-list.component.scss'
})
export class TransactionListComponent {
  @Input({ required: true }) transactions: Transaction[] = [];
  @Output() edit = new EventEmitter<Transaction>();
  @Output() delete = new EventEmitter<number>();

  protected trackById(_index: number, transaction: Transaction): number {
    return transaction.id;
  }

  protected onEdit(transaction: Transaction): void {
    this.edit.emit(transaction);
  }

  protected onDelete(id: number): void {
    this.delete.emit(id);
  }

  protected getCategoryLabel(transaction: Transaction): string {
    return transaction.categoryName || `Category #${transaction.categoryId}`;
  }
}
