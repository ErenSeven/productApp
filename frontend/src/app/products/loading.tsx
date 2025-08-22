export default function Loading() {
  return (
    <main className="mx-auto max-w-5xl px-4 py-8">
      <div className="mb-6 h-7 w-40 animate-pulse rounded bg-gray-200" />
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
        {Array.from({ length: 6 }).map((_, i) => (
          <div key={i} className="h-28 animate-pulse rounded-2xl bg-gray-100" />
        ))}
      </div>
    </main>
  );
}
