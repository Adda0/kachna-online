import { BaseEvent } from "./base-event.model";

export class EventModification extends BaseEvent {
  from: string;
  to: string;
}
