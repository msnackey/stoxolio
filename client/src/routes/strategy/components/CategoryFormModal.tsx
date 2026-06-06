import { Button, Group, Modal, NumberInput, TextInput } from '@mantine/core'
import { useForm } from '@mantine/form'
import { useEffect } from 'react'
import type Category from '../../../types/category'

interface Props {
  opened: boolean
  category?: Pick<Category, 'id' | 'name' | 'target'>
  onClose: () => void
  onSave: (values: Pick<Category, 'name' | 'target'>, id?: number) => void
}

export default function CategoryFormModal({ opened, category, onClose, onSave }: Props) {
  const form = useForm({
    initialValues: {
      name: category?.name ?? '',
      target: (category?.target ?? 0) * 100,
    },
  })

  useEffect(() => {
    if (opened) {
      form.setValues({
        name: category?.name ?? '',
        target: (category?.target ?? 0) * 100,
      })
      form.resetDirty()
    }
  }, [opened])

  const handleSubmit = (values: typeof form.values) => {
    onSave({ name: values.name, target: values.target / 100 }, category?.id)
    onClose()
  }

  const title = category ? 'Edit Category' : 'New Category'

  return (
    <Modal opened={opened} onClose={onClose} title={title} centered>
      <form onSubmit={form.onSubmit(handleSubmit)}>
        <TextInput label="Name" {...form.getInputProps('name')} mb="sm" />
        <NumberInput
          label="Target (%)"
          min={0}
          max={100}
          decimalScale={1}
          suffix="%"
          {...form.getInputProps('target')}
          mb="md"
        />
        <Group justify="flex-end">
          <Button variant="default" onClick={onClose}>
            Cancel
          </Button>
          <Button type="submit">Save</Button>
        </Group>
      </form>
    </Modal>
  )
}
