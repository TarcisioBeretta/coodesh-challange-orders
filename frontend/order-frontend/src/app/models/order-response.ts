export interface OrderResponse {
  execType: number;
  symbol: SymbolResponse;
  side: number;
  quantity: number;
  price: number;
  text: string | null;
}

export interface SymbolResponse {
  value: string;
}