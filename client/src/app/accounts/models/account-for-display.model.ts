import { GroupForDisplay } from '../groups/models/group-for-display.model';

export class AccountForDisplay {
  id: number;
  firstName: string;
  lastName: string;
  description: string;
  group: GroupForDisplay;
  email: string;
}