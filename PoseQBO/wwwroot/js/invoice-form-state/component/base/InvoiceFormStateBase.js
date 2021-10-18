class InvoiceFormStateBase {
    state;
    action

    get State() {
        return this.state;
    }

    set State(state) {
        this.state = state;
    }

    get Action() {
        return this.action;
    }

    set Action(action) {
        this.action = action;
    }
}

export default InvoiceFormStateBase;