import {Exception} from './Exception';
import {Metadata} from './Metadata';

export interface OperationResult<T> {
  result: T;
  ok: boolean;
  exception: Exception;
  metadata: Metadata;
}
