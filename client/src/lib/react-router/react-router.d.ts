import 'react-router-dom'

declare module 'react-router-dom' {
  interface Future {
    v8_middleware: true
  }
}
