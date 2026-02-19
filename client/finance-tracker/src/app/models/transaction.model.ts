export type TransactionType = 'income' | 'expense';

export interface Transaction {
  id: number;
  date: string;
  type: TransactionType;
  categoryId: number;
  categoryName?: string;
  amount: number;
  note?: string;
}

export interface TransactionFilters {
  from?: string;
  to?: string;
  type?: TransactionType;
}

export interface TransactionUpsertRequest {
  date: string;
  type: TransactionType;
  categoryId: number;
  amount: number;
  note?: string;
}
