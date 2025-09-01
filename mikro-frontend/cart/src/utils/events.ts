
export type EventPayload = {
  type: string;
  payload?: any;
};

export const dispatchCrossAppEvent = (eventName: string, data: EventPayload) => {
  if (typeof window !== "undefined") {
    window.dispatchEvent(
      new CustomEvent(eventName, {
        detail: data,
      })
    );
  }
};
