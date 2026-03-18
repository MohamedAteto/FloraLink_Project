import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { getDiaryEntries, addDiaryEntry, deleteDiaryEntry } from '../services/api'
import './GrowthDiary.css'

export default function GrowthDiary() {
  const { plantId } = useParams()
  const navigate = useNavigate()
  const [entries, setEntries] = useState([])
  const [form, setForm] = useState({ notes: '', photoUrl: '' })
  const [loading, setLoading] = useState(true)
  const [submitting, setSubmitting] = useState(false)

  const load = () =>
    getDiaryEntries(plantId).then(r => { setEntries(r.data); setLoading(false) })

  useEffect(() => { load() }, [plantId])

  const handleFileChange = (e) => {
    const file = e.target.files[0]
    if (!file) return
    const reader = new FileReader()
    reader.onload = (ev) => setForm(f => ({ ...f, photoUrl: ev.target.result }))
    reader.readAsDataURL(file)
  }

  const handleAdd = async (e) => {
    e.preventDefault()
    setSubmitting(true)
    try {
      await addDiaryEntry({ plantId: parseInt(plantId), ...form })
      setForm({ notes: '', photoUrl: '' })
      // reset file input
      const fileInput = document.getElementById('photoFile')
      if (fileInput) fileInput.value = ''
      await load()
    } finally {
      setSubmitting(false)
    }
  }

  const handleDelete = async (id) => {
    await deleteDiaryEntry(id)
    setEntries(prev => prev.filter(e => e.id !== id))
  }

  if (loading) return <div className="loading">Loading diary…</div>

  return (
    <div>
      <div className="page-header">
        <h1 className="page-title">📔 Growth Diary</h1>
        <button className="btn btn-secondary" onClick={() => navigate(`/plants/${plantId}`)}>← Back to Plant</button>
      </div>

      <div className="diary-layout">
        <div className="card diary-form">
          <h3 className="section-title">New Entry</h3>
          <form onSubmit={handleAdd}>
            <div className="form-group">
              <label htmlFor="notes">Notes</label>
              <textarea id="notes" rows={4} value={form.notes} onChange={e => setForm({ ...form, notes: e.target.value })} placeholder="How is your plant doing today?" />
            </div>
            <div className="form-group">
              <label htmlFor="photoFile">Photo Upload</label>
              <input id="photoFile" type="file" accept="image/*" onChange={handleFileChange} className="file-input" />
              {form.photoUrl && (
                <div className="photo-preview-wrap">
                  <img src={form.photoUrl} alt="Preview" className="photo-preview" />
                  <button type="button" className="btn btn-danger preview-remove" onClick={() => { setForm(f => ({ ...f, photoUrl: '' })); document.getElementById('photoFile').value = '' }}>✕</button>
                </div>
              )}
            </div>
            <button type="submit" className="btn btn-primary" style={{ width: '100%', justifyContent: 'center' }} disabled={submitting}>
              {submitting ? 'Saving…' : '+ Add Entry'}
            </button>
          </form>
        </div>

        <div className="diary-entries">
          {entries.length === 0 ? (
            <div className="card empty-state" style={{ padding: '40px', textAlign: 'center' }}>
              <div style={{ fontSize: 40 }}>📝</div>
              <p style={{ color: 'var(--gray-500)', marginTop: 8 }}>No diary entries yet. Start documenting your plant's journey!</p>
            </div>
          ) : (
            entries.map(entry => (
              <div key={entry.id} className="card diary-entry">
                <div className="entry-header">
                  <span className="entry-date">{new Date(entry.entryDate).toLocaleDateString('en', { year: 'numeric', month: 'long', day: 'numeric' })}</span>
                  <button className="btn btn-danger entry-delete" onClick={() => handleDelete(entry.id)}>Delete</button>
                </div>
                {entry.photoUrl && (
                  <img src={entry.photoUrl} alt="Plant photo" className="entry-photo" onError={e => e.target.style.display = 'none'} />
                )}
                {entry.notes && <p className="entry-notes">{entry.notes}</p>}
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  )
}
