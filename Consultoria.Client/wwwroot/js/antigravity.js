/**
 * Anti-Gravity Effect — Consultoria Dev
 * Mouse-tracking parallax with floating orbs and particle system.
 * Pure Vanilla JS — no external dependencies.
 */

// Global helper for Blazor JS Interop scroll calls
window.scrollToElement = function (id) {
  const el = document.getElementById(id);
  if (el) el.scrollIntoView({ behavior: 'smooth' });
};

(function () {
  'use strict';

  // ─── Configuration ───────────────────────────────────────────────────
  const CONFIG = {
    orbs: { inertia: 0.08, maxMove: 60 },
    cards: { inertia: 0.05, maxMove: 15, tiltMax: 8 },
    particles: { count: 25, colors: ['#7c3aed', '#00d4ff', '#f472b6', '#10b981'] }
  };

  let mouse = { x: 0, y: 0, targetX: 0, targetY: 0 };
  let animFrame;
  let particles = [];

  // ─── Mouse Tracking ───────────────────────────────────────────────────
  document.addEventListener('mousemove', (e) => {
    mouse.targetX = (e.clientX / window.innerWidth - 0.5) * 2;
    mouse.targetY = (e.clientY / window.innerHeight - 0.5) * 2;
  });

  // ─── Particle System ──────────────────────────────────────────────────
  function createParticle() {
    const particle = document.createElement('div');
    const size = Math.random() * 4 + 2;
    const color = CONFIG.particles.colors[Math.floor(Math.random() * CONFIG.particles.colors.length)];
    const duration = Math.random() * 8 + 6;
    const delay = Math.random() * 5;

    particle.style.cssText = `
      position: fixed;
      width: ${size}px;
      height: ${size}px;
      background: ${color};
      border-radius: 50%;
      pointer-events: none;
      z-index: 0;
      left: ${Math.random() * 100}vw;
      bottom: -10px;
      opacity: 0;
      box-shadow: 0 0 ${size * 2}px ${color};
      animation: particle-float ${duration}s ${delay}s linear infinite;
    `;

    document.body.appendChild(particle);
    return particle;
  }

  function initParticles() {
    for (let i = 0; i < CONFIG.particles.count; i++) {
      particles.push(createParticle());
    }
  }

  // ─── Floating Orb Parallax ────────────────────────────────────────────
  function animateOrbs() {
    mouse.x += (mouse.targetX - mouse.x) * CONFIG.orbs.inertia;
    mouse.y += (mouse.targetY - mouse.y) * CONFIG.orbs.inertia;

    document.querySelectorAll('.floating-element').forEach((el, i) => {
      const factor = (i + 1) * 0.4;
      const x = mouse.x * CONFIG.orbs.maxMove * factor;
      const y = mouse.y * CONFIG.orbs.maxMove * factor;

      const currentAnimation = el.style.animation;
      el.style.transform = `translate(${x}px, ${y}px)`;
    });

    animFrame = requestAnimationFrame(animateOrbs);
  }

  // ─── Card Tilt Effect ─────────────────────────────────────────────────
  function initCardTilt() {
    document.querySelectorAll('.problem-card').forEach(card => {
      card.addEventListener('mousemove', (e) => {
        const rect = card.getBoundingClientRect();
        const x = (e.clientX - rect.left) / rect.width - 0.5;
        const y = (e.clientY - rect.top) / rect.height - 0.5;

        const rotateX = y * CONFIG.cards.tiltMax * -1;
        const rotateY = x * CONFIG.cards.tiltMax;

        card.style.transform =
          `translateY(-8px) scale(1.02) rotateX(${rotateX}deg) rotateY(${rotateY}deg)`;
      });

      card.addEventListener('mouseleave', () => {
        card.style.transform = '';
        card.style.transition = 'transform 0.6s cubic-bezier(0.23, 1, 0.32, 1)';
      });

      card.addEventListener('mouseenter', () => {
        card.style.transition = 'transform 0.1s ease-out, border-color 0.4s, box-shadow 0.4s';
      });
    });
  }

  // ─── Scroll Reveal ────────────────────────────────────────────────────
  function initScrollReveal() {
    const observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          entry.target.style.opacity = '1';
          entry.target.style.transform = 'translateY(0)';
          observer.unobserve(entry.target);
        }
      });
    }, { threshold: 0.1 });

    document.querySelectorAll('.problem-card, .summary-card').forEach((el, i) => {
      el.style.opacity = '0';
      el.style.transform = 'translateY(30px)';
      el.style.transition = `opacity 0.6s ease ${i * 0.1}s, transform 0.6s cubic-bezier(0.23, 1, 0.32, 1) ${i * 0.1}s`;
      observer.observe(el);
    });
  }

  // ─── Initialize ───────────────────────────────────────────────────────
  function init() {
    initParticles();
    animateOrbs();

    // Re-initialize on Blazor navigation (Blazor re-renders DOM)
    setTimeout(() => {
      initCardTilt();
      initScrollReveal();
    }, 500);
  }

  // Wait for Blazor to be ready
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }

  // Expose for Blazor JS Interop
  window.AntiGravity = {
    reinitialize: function () {
      setTimeout(() => {
        initCardTilt();
        initScrollReveal();
      }, 100);
    }
  };

})();
