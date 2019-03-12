const tools = {
  namespaced: true,
  state: {
    drawer: false
  },
  mutations: {
    switchDrawer(state) {
      state.drawer = !state.drawer;
    }
  },
  actions: {
    switchDrawer(state) {
      state.commit('switchDrawer');
    }
  }
}

export default tools;