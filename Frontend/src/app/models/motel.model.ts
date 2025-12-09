import { Suite } from './suite.model';

export interface Motel {
  id?: number;
  name?: string;
  address?: string;
  phone?: string;
  description?: string;
  suites?: Suite[];
}
