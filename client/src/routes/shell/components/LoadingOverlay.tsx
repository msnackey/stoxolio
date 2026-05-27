import { LoadingOverlay } from '@mantine/core'

type LoadingOverlayComponentProps = {
  visible?: boolean
}

export default function LoadingOverlayComponent({ visible = true }: LoadingOverlayComponentProps) {
  return (
    <LoadingOverlay
      visible={visible}
      zIndex={1000}
      overlayProps={{ blur: 2 }}
      loaderProps={{ size: 'xl', color: 'green' }}
    />
  )
}
